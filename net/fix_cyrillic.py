#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Script to fix corrupted Cyrillic text in .cs files by matching with old project.
Only fixes text in comments, string literals, and region names.
"""

import os
import re
import sys
from pathlib import Path

# Paths
NEW_PROJECT_ROOT = Path(r"C:\Repos\github\sql-builder-web\net\SqlBuilder")
OLD_PROJECT_ROOT = Path(r"C:\Repos\ai-tfs\root\main\all\sql.builder")

def decode_windows1251_to_utf8(text):
    """Convert Windows-1251 encoded Cyrillic text to UTF-8."""
    try:
        # Text is already read as UTF-8 but contains Windows-1251 bytes interpreted as Latin-1
        # We need to encode as Latin-1 first (to get the bytes), then decode as Windows-1251
        return text.encode('latin1').decode('windows-1251')
    except:
        try:
            # Alternative: if text is already bytes-like, decode directly
            if isinstance(text, bytes):
                return text.decode('windows-1251')
            return text
        except:
            return text

def extract_text_content(line):
    """Extract text content from comments, string literals, or region names."""
    # Single-line comment: // ...
    comment_match = re.match(r'^(\s*//\s*)(.*)$', line)
    if comment_match:
        return comment_match.group(2)
    
    # Multi-line comment start: /* ...
    comment_match = re.match(r'^(\s*/\*\s*)(.*)$', line)
    if comment_match:
        return comment_match.group(2)
    
    # String literal: "..." or @"..."
    string_match = re.search(r'@"([^"]*)"|"([^"]*)"', line)
    if string_match:
        return string_match.group(1) or string_match.group(2)
    
    # Region directive: #region ... or #endregion ...
    region_match = re.match(r'^(\s*#(?:region|endregion)\s+)(.*)$', line)
    if region_match:
        return region_match.group(2)
    
    return None

def is_corrupted_cyrillic(text):
    """Check if text contains corrupted Cyrillic patterns."""
    if not text:
        return False
    
    # Check for common corruption patterns
    corruption_patterns = [
        r'[а-яА-Я].*[?]{2,}',  # Cyrillic followed by multiple question marks
        r'[?]{3,}',  # Multiple question marks
        r'[а-яА-Я].*[�]',  # Cyrillic with replacement characters
    ]
    
    for pattern in corruption_patterns:
        if re.search(pattern, text):
            return True
    
    # Check if text looks like corrupted encoding (garbled characters)
    # This is a heuristic - if text has Cyrillic-like patterns but looks wrong
    if re.search(r'[а-яА-Я]', text):
        # If it has Cyrillic but also has weird characters, might be corrupted
        if re.search(r'[^\w\s\.,;:!?\-\(\)\[\]\{\}\"\']', text):
            return True
    
    return False

def get_context_lines(lines, line_index, context_size=3):
    """Get context lines around a given line index."""
    start = max(0, line_index - context_size)
    end = min(len(lines), line_index + context_size + 1)
    return lines[start:end], line_index - start

def normalize_for_matching(line):
    """Normalize a line for matching (remove whitespace differences, etc.)."""
    # Remove leading/trailing whitespace
    line = line.strip()
    # Normalize whitespace
    line = re.sub(r'\s+', ' ', line)
    return line

def find_matching_line_in_old_project(new_file_path, new_line_index, new_lines, context_size=5):
    """Find matching line in old project by context."""
    # Get relative path
    rel_path = new_file_path.relative_to(NEW_PROJECT_ROOT)
    old_file_path = OLD_PROJECT_ROOT / rel_path
    
    if not old_file_path.exists():
        return None, None
    
    # Try reading as Windows-1251 first (most likely encoding for Cyrillic)
    try:
        with open(old_file_path, 'r', encoding='windows-1251', errors='ignore') as f:
            old_lines = f.readlines()
    except:
        try:
            with open(old_file_path, 'r', encoding='utf-8', errors='ignore') as f:
                old_lines = f.readlines()
        except:
            return None, None
    
    # Get context around the new line
    new_context, new_offset = get_context_lines(new_lines, new_line_index, context_size)
    new_context_normalized = [normalize_for_matching(line) for line in new_context]
    
    # Try to find matching context in old file
    for old_line_index in range(len(old_lines)):
        old_context, old_offset = get_context_lines(old_lines, old_line_index, context_size)
        old_context_normalized = [normalize_for_matching(line) for line in old_context]
        
        # Check if contexts match (allowing for some differences)
        if len(new_context_normalized) == len(old_context_normalized):
            matches = sum(1 for n, o in zip(new_context_normalized, old_context_normalized) 
                         if n == o or (n and o and (n in o or o in n)))
            if matches >= len(new_context_normalized) - 1:  # Allow 1 line difference
                return old_lines, old_line_index
    
    return None, None

def fix_line(new_line, old_line):
    """Fix a corrupted line using the old line as reference."""
    new_text = extract_text_content(new_line)
    old_text = extract_text_content(old_line)
    
    if not new_text or not old_text:
        return new_line
    
    # If old text looks like Windows-1251 interpreted as Latin-1 (has accented chars), convert it
    # Check if old_text contains characters that look like Windows-1251 Cyrillic
    try:
        # Try to decode: encode as latin1 (to get bytes), then decode as windows-1251
        old_text_bytes = old_text.encode('latin1')
        old_text_utf8 = old_text_bytes.decode('windows-1251')
    except:
        # If conversion fails, use old_text as-is (might already be UTF-8)
        old_text_utf8 = old_text
    
    # Replace corrupted text with correct text
    if '//' in new_line and new_line.strip().startswith('//'):
        # Single-line comment
        match = re.match(r'^(\s*//\s*)(.*?)(\r?\n?)$', new_line)
        if match:
            prefix = match.group(1)
            suffix = match.group(3) if match.group(3) else '\n'
            return prefix + old_text_utf8 + suffix
    elif '/*' in new_line and new_line.strip().startswith('/*'):
        # Multi-line comment start
        match = re.match(r'^(\s*/\*\s*)(.*?)(\r?\n?)$', new_line)
        if match:
            prefix = match.group(1)
            suffix = match.group(3) if match.group(3) else '\n'
            return prefix + old_text_utf8 + suffix
    elif '"' in new_line:
        # String literal
        # Find the string literal and replace its content
        if '@"' in new_line:
            match = re.search(r'(@")([^"]*)(")', new_line)
            if match:
                return new_line[:match.start(2)] + old_text_utf8 + new_line[match.end(2):]
        else:
            match = re.search(r'(")([^"]*)(")', new_line)
            if match:
                return new_line[:match.start(2)] + old_text_utf8 + new_line[match.end(2):]
    elif new_line.strip().startswith('#region') or new_line.strip().startswith('#endregion'):
        # Region directive
        match = re.match(r'^(\s*#(?:region|endregion)\s+)(.*?)(\r?\n?)$', new_line)
        if match:
            prefix = match.group(1)
            suffix = match.group(3) if match.group(3) else '\n'
            return prefix + old_text_utf8 + suffix
    
    return new_line

def process_file(file_path):
    """Process a single .cs file."""
    print(f"Processing: {file_path.relative_to(NEW_PROJECT_ROOT)}")
    
    try:
        with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
            lines = f.readlines()
    except:
        print(f"  Error reading file: {file_path}")
        return False
    
    modified = False
    new_lines = []
    
    for i, line in enumerate(lines):
        text_content = extract_text_content(line)
        
        if text_content and is_corrupted_cyrillic(text_content):
            print(f"  Found corrupted line {i+1}: {line.strip()[:80]}")
            
            # Try to find matching line in old project
            old_lines, old_line_index = find_matching_line_in_old_project(file_path, i, lines)
            
            if old_lines and old_line_index is not None:
                old_line = old_lines[old_line_index]
                fixed_line = fix_line(line, old_line)
                
                if fixed_line != line:
                    print(f"    Fixed: {fixed_line.strip()[:80]}")
                    new_lines.append(fixed_line)
                    modified = True
                else:
                    new_lines.append(line)
            else:
                # No match found - clean corrupted symbols
                print(f"    No match found, cleaning corrupted symbols")
                # Remove corrupted characters but keep the line structure
                cleaned = re.sub(r'[?]{2,}', '', text_content)
                cleaned = re.sub(r'[^\w\s\.,;:!?\-\(\)\[\]\{\}\"\']', '', cleaned)
                
                if line.startswith('//'):
                    prefix = re.match(r'^(\s*//\s*)', line).group(1)
                    new_lines.append(prefix + cleaned + '\n')
                else:
                    new_lines.append(line)
                modified = True
        else:
            new_lines.append(line)
    
    if modified:
        # Write back the file
        try:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.writelines(new_lines)
            print(f"  File updated")
            return True
        except Exception as e:
            print(f"  Error writing file: {e}")
            return False
    
    return False

def main():
    """Main function to process all .cs files."""
    if not NEW_PROJECT_ROOT.exists():
        print(f"Error: New project root not found: {NEW_PROJECT_ROOT}")
        return
    
    if not OLD_PROJECT_ROOT.exists():
        print(f"Error: Old project root not found: {OLD_PROJECT_ROOT}")
        return
    
    print(f"Scanning for .cs files in {NEW_PROJECT_ROOT}")
    
    cs_files = list(NEW_PROJECT_ROOT.rglob("*.cs"))
    print(f"Found {len(cs_files)} .cs files")
    
    fixed_count = 0
    for cs_file in cs_files:
        if process_file(cs_file):
            fixed_count += 1
    
    print(f"\nFixed {fixed_count} files")

if __name__ == "__main__":
    main()
