const fs = require('fs');
const path = require('path');

/**
 * Recursively finds all .cs files in a directory
 * @param {string} dirPath - Directory to search
 * @param {string} baseDir - Base directory for relative paths
 * @param {Set<string>} fileSet - Set to collect file paths (relative to baseDir)
 */
function findCsFiles(dirPath, baseDir, fileSet = new Set()) {
    try {
        const entries = fs.readdirSync(dirPath, { withFileTypes: true });
        
        for (const entry of entries) {
            const fullPath = path.join(dirPath, entry.name);
            
            if (entry.isDirectory()) {
                findCsFiles(fullPath, baseDir, fileSet);
            } else if (entry.isFile() && entry.name.endsWith('.cs')) {
                const relativePath = path.relative(baseDir, fullPath).replace(/\\/g, '\\');
                fileSet.add(relativePath);
            }
        }
    } catch (error) {
        console.error(`Error reading directory ${dirPath}:`, error.message);
    }
    
    return fileSet;
}

/**
 * Normalizes a path from .csproj format to match file system paths
 */
function normalizePath(csprojPath) {
    return csprojPath.replace(/\//g, '\\');
}

/**
 * Main function to clean up .csproj file
 */
function main() {
    const sqlBuilderDir = path.join(__dirname, 'sql.builder');
    const csprojPath = path.join(sqlBuilderDir, 'sql.builder.csproj');
    
    // Check if directories exist
    if (!fs.existsSync(sqlBuilderDir)) {
        console.error(`Directory not found: ${sqlBuilderDir}`);
        process.exit(1);
    }
    
    if (!fs.existsSync(csprojPath)) {
        console.error(`File not found: ${csprojPath}`);
        process.exit(1);
    }
    
    console.log('Finding all existing .cs files...');
    const existingCsFiles = findCsFiles(sqlBuilderDir, sqlBuilderDir);
    console.log(`Found ${existingCsFiles.size} existing .cs files\n`);
    
    // Read the .csproj file
    console.log('Reading .csproj file...');
    const lines = fs.readFileSync(csprojPath, 'utf8').split(/\r?\n/);
    
    const cleanedLines = [];
    let compileRemoved = 0;
    let embeddedResourceRemoved = 0;
    let i = 0;
    
    while (i < lines.length) {
        const line = lines[i];
        const trimmed = line.trim();
        
        // Check for Compile entries
        const compileMatch = trimmed.match(/<Compile Include="([^"]+\.cs)"/);
        if (compileMatch) {
            const csPath = normalizePath(compileMatch[1]);
            
            if (!existingCsFiles.has(csPath)) {
                // Skip this Compile entry and its closing tag
                compileRemoved++;
                // Skip until we find the closing tag (could be self-closing or separate)
                if (trimmed.includes('/>')) {
                    // Self-closing, already skipped
                    i++;
                    continue;
                } else {
                    // Multi-line entry, skip until closing tag
                    i++;
                    while (i < lines.length && !lines[i].trim().includes('</Compile>')) {
                        i++;
                    }
                    if (i < lines.length) i++; // Skip closing tag
                    continue;
                }
            }
        }
        
        // Check for EmbeddedResource entries
        const embeddedResourceMatch = trimmed.match(/<EmbeddedResource Include="([^"]+\.resx)"/);
        if (embeddedResourceMatch) {
            const resxPath = embeddedResourceMatch[1];
            const resxDir = path.dirname(resxPath);
            
            // Look ahead for DependentUpon
            let dependentUponCsFile = null;
            let j = i;
            while (j < lines.length && j < i + 10) { // Check next 10 lines
                const dependentMatch = lines[j].match(/<DependentUpon>([^<]+\.cs)<\/DependentUpon>/);
                if (dependentMatch) {
                    dependentUponCsFile = dependentMatch[1];
                    break;
                }
                // Check if we've moved to next entry
                if (j > i && lines[j].trim().startsWith('<') && !lines[j].includes('DependentUpon')) {
                    break;
                }
                j++;
            }
            
            if (dependentUponCsFile) {
                // Construct full path
                const csFilePath = resxDir ? `${normalizePath(resxDir)}\\${dependentUponCsFile}` : dependentUponCsFile;
                
                if (!existingCsFiles.has(csFilePath)) {
                    // Skip this EmbeddedResource entry
                    embeddedResourceRemoved++;
                    // Skip until closing tag
                    if (trimmed.includes('/>')) {
                        i++;
                        continue;
                    } else {
                        i++;
                        while (i < lines.length && !lines[i].trim().includes('</EmbeddedResource>')) {
                            i++;
                        }
                        if (i < lines.length) i++; // Skip closing tag
                        continue;
                    }
                }
            }
        }
        
        // Keep this line
        cleanedLines.push(line);
        i++;
    }
    
    // Write back to file
    console.log('Writing cleaned .csproj file...');
    fs.writeFileSync(csprojPath, cleanedLines.join('\r\n'), 'utf8');
    
    console.log('\n' + '='.repeat(80));
    console.log('SUMMARY:');
    console.log('='.repeat(80));
    console.log(`Compile entries removed: ${compileRemoved}`);
    console.log(`EmbeddedResource entries removed: ${embeddedResourceRemoved}`);
    console.log(`Total entries removed: ${compileRemoved + embeddedResourceRemoved}`);
    console.log('\n.csproj file cleaned successfully!');
}

// Run the script
main();
