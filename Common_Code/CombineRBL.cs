using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
//////////////////////////////////////////////////////////////////////////////////////////
//  This custom web control was created to slightly change the original RadioButtonList.
//  The control allows you to manual set the grouping name on the radio buttons.  This
//  allows you to have multiple CombineRBL controls with all their radiobuttons grouped
//  together.  NOTE:  You should group multiple CombineRBL controls only if you are sure
//  that the data used as values are unique for all.  The first use of this is on the
//  Child Report selection page.  Secreening reports are in one CombineRBL and Complete
//  reporst are in the other.  Each report has a unique reportid so they can safely be
//  grouped together. 
//////////////////////////////////////////////////////////////////////////////////////////

namespace BDI2Web.Common_Code
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:CombineRBL runat=server></{0}:CombineRBL>")]
    public class CombineRBL : RadioButtonList, IPostBackDataHandler

    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]


        bool need_raise;

        public string GroupName
        {
            get
            {
                return _GroupName;
            }

            set
            {
                _GroupName = value;
            }
        }
        private string _GroupName
        {
            get
            {
                String s = (String)ViewState["GroupName"];
                return ((s == null) ? "DefaultName" : s);
            }

            set
            {
                ViewState["GroupName"] = value;
            }
        }

        protected override void  OnInit(EventArgs e)
        {
 	        base.OnInit(e);
            Page.RegisterRequiresPostBack(this); 
        }
        

        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            base.Render(hw);

            String strPattern = "(<input.*?name=\\\")(.*?)(\\\".*?>)";
            String strInsert = "$1" + _GroupName + "$3";
            //String strInsert = "$1$2$3";
            Regex rx = new Regex(strPattern);
            String strWork = rx.Replace(sb.ToString(), strInsert);

            output.Write(strWork);
            //output.Write(sb.ToString());
        }

        protected override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {

            //string val = postCollection[postDataKey];
            string val = postCollection[_GroupName];
            ListItemCollection items = Items;
            int end = items.Count;
            int selected = SelectedIndex;
            for (int i = 0; i < end; i++)
            {
                ListItem item = items[i];
                if (item == null || val != item.Value)
                    continue;
                if (i != selected)
                {
                    SelectedIndex = i;
                    need_raise = true;
                }
                return true;
            }
            SelectedIndex = -1;
            return false;
        }

        protected override void RaisePostDataChangedEvent()
        {
            if (need_raise)
                OnSelectedIndexChanged(
                EventArgs.Empty);
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            return LoadPostData(postDataKey, postCollection);
               
        }


        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            RaisePostDataChangedEvent();
            
        }

        //public void RaisePostBackEvent(string eventArgument)
        //{

        //    OnSelectedIndexChanged(new EventArgs());
        //    //OnClick(new EventArgs());
        //}


    }
}
