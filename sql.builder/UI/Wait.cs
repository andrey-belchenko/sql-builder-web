using System;
using System.Collections.Generic;
using System.Linq;
//using System.Windows.Forms; // Cursor, Cursors
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraWaitForm;
using Point = System.Drawing.Point;
//using WaitForm = infoenergo.ui.win.WaitForm;

namespace sql.builder
{
    internal static class Wait 
    {
        private class WaitUIInfo
        {
            internal string Caption;
            internal bool IsCursor;
            internal bool Overlap;
            internal bool Overlaped;
            internal string Description;
            internal DateTime Start;
            internal bool Delayed;
        }
        private static SortedDictionary<int, WaitUIInfo> _stack = new SortedDictionary<int, WaitUIInfo>();
        private static object _lock = new object();
        private static int _idCounter; // должно пригодиться для многопоточности
        private static WaitUIInfo _current;
        internal static bool Waiting()
        {
            return _current != null;
        }
        internal static void Check()
        {
            lock (_lock) {
                if (_current != null) {
                    if (_current.Delayed) {
                        if (_current.Start < DateTime.Now) {
                            _current.Delayed = false;
                            _current.IsCursor = false;
                            hideCursor();
                            showPanel(_current);
                        }
                    }
                }
            }
        }
        internal static int ShowPanel(string capition, bool overlap, int delayMilliseconds, string description = null)
        {
            lock (_lock) {
                if (description == null) {
                    description = "Пожалуйста, подождите...";
                }
                var info = new WaitUIInfo();
                info.Caption = capition;
                info.Description = description;
                info.Overlap = overlap;
                info.Start = DateTime.Now.AddMilliseconds(delayMilliseconds);
                info.Delayed = DateTime.Now < info.Start;
                info.IsCursor = info.Delayed;
                _idCounter++;
                show(_idCounter, info);
                return _idCounter;
            }
        }
        /*internal static int ShowCursor(bool overlap)
        {
            lock (_lock) {
                var info = new WaitUIInfo();
                info.Overlap = overlap;
                info.IsCursor = true;
                _idCounter++;
                show(_idCounter, info);
                return _idCounter;
            }
        }*/
        internal static void Hide(int id)
        {
            lock (_lock) {
                _stack.Remove(id);
                WaitUIInfo info = _stack.Values.LastOrDefault();
                if (info == null || info.Overlaped) {
                    change(info);
                }
            }
        }
        private static void show(int id, WaitUIInfo nextInfo)
        {
            _stack.Add(id, nextInfo);
            if (_current == null || !_current.Overlap) {
                change(nextInfo);
            } else {
                nextInfo.Overlaped = true;
            }
        }
        private static void change(WaitUIInfo newInfo)
        {
            WaitUIInfo oldInfo = _current;
            if (oldInfo == newInfo) return;
            if (newInfo == null) {
                if (oldInfo != null) {
                    if (oldInfo.IsCursor) {
                        hideCursor();
                    } else {
                        hidePanel();
                    }
                }
            } else {
                if (oldInfo == null) {
                    if (newInfo.IsCursor) {
                        showCursor();
                    } else {
                        showPanel(newInfo);
                    }
                } else {
                    if (oldInfo.IsCursor) {
                        if (!newInfo.IsCursor) {
                            hideCursor();
                            showPanel(newInfo);
                        }
                    } else {
                        if (!newInfo.IsCursor) {
                            showPanel(newInfo);
                        } else { //панель на курсор не меняем
                            newInfo.Overlaped = true;
                            newInfo = oldInfo;
                        }
                    }
                }
            }
            _current = newInfo;
        }
        private static void showCursor()
        {
             //Cursor.Current = Cursors.WaitCursor;
        }
        private static void hideCursor()
        {
            //Cursor.Current = Cursors.Default;
        }
        private static void showPanel(WaitUIInfo info)
        {
            //WaitForm.Close();
            //WaitForm.Show(info.Caption, info.Description);
        }
        private static void hidePanel()
        {
            //WaitForm.Close();
        }
    }
}