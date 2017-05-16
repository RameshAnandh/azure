using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using AjaxControlToolkit;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
public partial class SiteMaster : System.Web.UI.MasterPage
{
   public static SqlConnection c1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
  
   protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
            if (c1.State == System.Data.ConnectionState.Closed)
            {
                c1.Open();
            }
            SqlCommand cmenu = new SqlCommand("select * from Menu", c1);
            SqlDataReader dr = cmenu.ExecuteReader();
            while (dr.Read())
            {
               NavigationMenu.Items.Add(new MenuItem(dr["menutext"].ToString(),
               dr["menutext"].ToString(), "", dr["Url"].ToString()));
            }
            dr.Close();
            ///////////////
            #region Comment
            //SqlCommand cmenucolor = new SqlCommand("select top 1 MenuColor from MenuColor order by Id DESC", c1);
            //SqlDataReader dr1 = cmenucolor.ExecuteReader();
            //if (dr1.Read())
            //{
            //    string color_val = dr1[0].ToString();
            //    string _aa_=Color.AliceBlue.Name;

            //  //  NavigationMenu.DynamicMenuItemStyle.BackColor =_aa_;
            //    //NavigationMenu.BackColor=
            //}
            //else
            //{ }
            //dr1.Close(); 
            #endregion
            ///////////////

        }
        else
        {

        }
    }

   [WebMethod]
   [ScriptMethod]
   public static Slide[] GetImages()
   {
       List<Slide> slides = new List<Slide>();
       string path = HttpContext.Current.Server.MapPath("~/Image/");
       if (path.EndsWith("\\"))
       {
           path = path.Remove(path.Length - 1);
       }
       Uri pathUri = new Uri(path, UriKind.Absolute);
       string[] files = Directory.GetFiles(path);
       foreach (string file in files)
       {
           Uri filePathUri = new Uri(file, UriKind.Absolute);
           slides.Add(new Slide
           {
               Name = Path.GetFileNameWithoutExtension(file),
               Description = Path.GetFileNameWithoutExtension(file) + " Description.",
               ImagePath = pathUri.MakeRelativeUri(filePathUri).ToString()
           });
       }
       return slides.ToArray();
   }

}
