using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
//using System.Windows.Forms;
using Devart.Data.Oracle;
using sql.builder.UI;
using System.Reflection;
using sql.builder.Exceptions;
namespace sql.builder.DataApi
{
    internal partial class VForm
    {
        private bool IsNeedPreprocess()
        {
            /*if (this.Multireferences().Count != 0) {
                return true;// пока только Multireference возможно понадобится еще что-нибудь по той же технологии
            } else*/ if (this.ContentExpressions().Count != 0) {
                return true;
            }
            return false;
        }
        private List<VCall> ContentExpressions()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VCall>);
            }
            var list = GetContentSections().SelectMany(c => c.GetDescedantsP(EName.call).Where(cc=>cc.P_Table!="")).Cast<VCall>().ToList();

            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;

        }

        /*private List<VMultireference> Multireferences()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VMultireference>);
            }
            var list=GetDescedantsP(EName.multireference).Select(e=>(VMultireference)e).ToList();

            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;

        }
        private VMultireference GetMultireference(string table,string link)
        {
            return Multireferences().Where(e => (e.P_CalledQuery == link) && (e.P_Table == table)).FirstOrDefault();
        }*/

        private VCall GetContentExpression(string table, string alias)
        {

            return ContentExpressions().Where(e => (e.P_Alias == alias) && (e.P_Table == table)).FirstOrDefault();
        }

        /*private void PreprocessMulireferences(XElement xform1)
        {
            foreach (XElement mr in xform1.Descendants(TextConst.EName.Multireference).ToArray())
            {
                var vmr = GetMultireference(mr.Attribute(TextConst.AName.Table).Value, mr.Attribute(TextConst.AName.Link).Value);
                var xdummyCol = new XElement(TextConst.AName.Column);
                Cmn.copyAttributes(mr, xdummyCol);
                xdummyCol.Attribute(TextConst.AName.Link).Remove();
                var alias = vmr.XName;

                xdummyCol.SetAttributeValue(TextConst.AName.As, alias);
                xdummyCol.SetAttributeValue(TextConst.AName.Type, vmr.XDataType());
                xdummyCol.SetAttributeValue(TextConst.AName.Title, vmr.P_Title);

                xdummyCol.SetAttributeValue(TextConst.AName.Editable, vmr.P_Editable);
                if (vmr.P_ColumnEditable == "" && vmr.P_Editable == "")
                {
                    xdummyCol.SetAttributeValue(TextConst.AName.ColumnEditable, TextConst.AVBool.True);
                }

                if (vmr.P_ControlType == "")
                {
                    xdummyCol.SetAttributeValue(TextConst.AName.ControlType, TextConst.AVControlType.List);
                }
                xdummyCol.Add(mr.Elements());
                xdummyCol.SetAttributeValue(TextConst.AName.Column, TextConst.AVColumn.Dummy);

                if (!xdummyCol.Elements(TextConst.EName.ListQuery).Any())
                {
                    var lq = new XElement(TextConst.EName.ListQuery);
                    var qry = new XElement(TextConst.EName.Query);
                    var slName = vmr.TypeQuery().XName;
                    qry.SetAttributeValue(TextConst.AName.Name, slName);
                    lq.Add(qry);
                    xdummyCol.Add(lq);
                }

                mr.AddAfterSelf(xdummyCol);


                if (vmr.P_TextSource == "")
                {
                    var xtextCol = new XElement(TextConst.EName.Fact);
                    xtextCol.SetAttributeValue(TextConst.AName.Table, vmr.P_Table);
                    xtextCol.SetAttributeValue(TextConst.AName.Color, vmr.P_BackColor);
                    xtextCol.SetAttributeValue(TextConst.AName.FontColor, vmr.P_FontColor);
                    var qry = (vmr.SourceColumn().First() as VColumn).TypeQuery();
                    var namecols = qry.NameColumns();
                    var textFactName = "";
                    if (namecols.Count == 1) {
                        textFactName = namecols.First().P_Fact;
                    }
                    if (textFactName == "") {
                        var s = "Для реализации множественного выбора у " + qry.XName + " должна быть определена одна колонка с именем с указанием факта с аггрегацией list";
                        //throw new NotImplementedException("У " + qry.XName + " должна быть определена одна колонка с именем с указанием факта с аггрегацией list");
                        throw new VCompilerException(s, this, vmr);
                    }


                    xtextCol.SetAttributeValue(TextConst.AName.Column, textFactName);
                    string textAlias = alias + TextConst.Pfx.ExtValName;
                    //  xtextCol.SetAttributeValue(TextConst.AName.As, textAlias);
                    xtextCol.SetAttributeValue(TextConst.AName.ParName, textAlias);

                    xdummyCol.SetAttributeValue(TextConst.AName.TextSource, textAlias);
                    xdummyCol.AddAfterSelf(xtextCol);

                }

                var xlink = new XElement(TextConst.EName.ELink);

                xlink.SetAttributeValue(TextConst.AName.Name, vmr.P_CalledQuery);

                xlink.SetAttributeValue(TextConst.AName.As, alias);
                xlink.SetAttributeValue(TextConst.AName.NewRowsVisForOtherTbls, TextConst.AVBool.True);
                Cmn.CopyAttribute(mr, xlink, TextConst.AName.Column);
                var xqryCall = xform1.Element(TextConst.EName.From).Descendants().Where(
                    e =>
                        ((XAttribute)Cmn.Nvl(e.Attribute(TextConst.AName.As), e.Attribute(TextConst.AName.Name))).Value == mr.Attribute(TextConst.AName.Table).Value
                    ).First();
                xqryCall.Add(xlink);
                mr.Remove();
                //                    <column table="vr_tree_sprav" column="dummy" type="number" as="test44" controlType="UIList" column-editable="1" textsource="ttt" exclude="1">
                //  <listquery>
                //    <query name="ips_klass_titul" />
                //  </listquery>
                //</column>
                //<elink name="vr_sprav_klass_titul" as="test44" new-rows-vis-for-other-tbls="1" column="kod_klass" exclude="1" />

            }
        }*/



        private void PreprocessExpressions(XElement xform1)
        {

            var list = xform1.Descendants(TextConst.EName.Call).Where(c=>Cmn.GetAttrValue(c,TextConst.AName.Table)!="").ToList();
            foreach (XElement xcall in list)
            {
                var vcall = GetContentExpression(xcall.Attribute(TextConst.AName.Table).Value, xcall.Attribute(TextConst.AName.As).Value);
                
                var xdummyCol = new XElement(TextConst.AName.Column);
                Cmn.copyAttributes(xcall, xdummyCol);
                xdummyCol.Attributes(TextConst.AName.Function).Remove();



                xdummyCol.SetAttributeValue(TextConst.AName.Column, TextConst.AVColumn.Dummy);
                xdummyCol.SetAttributeValue(TextConst.AName.Type, vcall.XDataType());
                xdummyCol.SetAttributeValue(TextConst.AName.Title, vcall.P_Title);
                xdummyCol.Add(xcall);
                xcall.AddAfterSelf(xdummyCol);
                xcall.Remove();//?
            }
        }

        public VForm GetPreprocessed()

        {
            if (!IsNeedPreprocess())
            {
                return this;
            }
            else
            {
                var xform1 = AsXElementApplyingParts();


                /*if (Multireferences().Any())
                {
                    PreprocessMulireferences(xform1);
                }*/

                if (ContentExpressions().Any())
                {
                    PreprocessExpressions(xform1);
                }


                var vform1 = VSXElement.Get<VForm>(xform1);

                //vform1.environment = this.GetEnvironment();
                vform1.VirtualParent = this.GetParent();
                return vform1;

            }
            
        }
    }
}
