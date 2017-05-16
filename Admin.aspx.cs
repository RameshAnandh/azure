using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
public partial class Admin : System.Web.UI.Page
{
    SqlConnection c1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        c1.Open();
        PanelMenu.Visible =PanelMenuColor.Visible=PanelSlide.Visible= false;
    }
    public void databind(string query)
    {
        SqlCommand cm = new SqlCommand(query, c1);
        SqlDataAdapter da = new SqlDataAdapter(cm);
        DataTable dt = new DataTable();
        da.Fill(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();        
    }

    public bool sqlinsert(string query)
    {
        SqlCommand cm = new SqlCommand(query,c1);
        int r0 = cm.ExecuteNonQuery();
        if (r0 > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void delete(string query)
    {
        SqlCommand cm1 = new SqlCommand(query, c1);
        int r = cm1.ExecuteNonQuery();
        if (r > 0)
        {
            Response.Write("<Script>alert('Deleted')</Script>");
            databind("select * from Menu");
        }
        else
        {
            Response.Write("<Script>alert('Error..Deleted')</Script>");
        }
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonList1.SelectedItem.Text == "Menu Item")
        {
            PanelMenu.Visible =PanelMenuColor.Visible=true;
            databind("select * from Menu");
        }
        else if (RadioButtonList1.SelectedItem.Text == "SlideShow Extender")
        {
            PanelSlide.Visible = true;
            databind("select * from Slide");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        bool r = sqlinsert("insert into Menu values('"+txtMenu.Text+"','"+txturl0.Text+"')");
        if (r)
        {
            Response.Write("<Script>alert('Added')</Script>");
            databind("select * from Menu");
        }
        else
        {
            Response.Write("<Script>alert('Error.Try Later')</Script>");
        }
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (RadioButtonList1.SelectedItem.Text == "Menu Item")
        {
            delete("delete from Menu where menutext='" + GridView1.Rows[e.RowIndex].Cells[1].Text + "' and Url='" + GridView1.Rows[e.RowIndex].Cells[2].Text + "'");
        }
        else if (RadioButtonList1.SelectedItem.Text == "SlideShow Extender")
        {
            string fie=GridView1.Rows[e.RowIndex].Cells[1].Text;
            delete("delete from Slide where Slide='" + fie + "'");
            /////
            string file_name = fie;
            string path = Server.MapPath("Image//" + file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();               
            }
            else
            {
               
            }
            ///////
            databind("select * from Slide");

        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        bool r = sqlinsert("insert into MenuColor values('" + DrMenuColor.SelectedItem.Text + "')");
        if (r)
        {
            Response.Write("<Script>alert('Added')</Script>");
          //  databind("select * from MenuColor");
        }
        else
        {
            Response.Write("<Script>alert('Error.Try Later')</Script>");
        }
    }
    protected void SlideAdd_Click(object sender, EventArgs e)
    {
        //int _count = 0;
        //string _query0 = "select count(*) from Slide";
        //SqlCommand cm0 = new SqlCommand(_query0, c1);
        //SqlDataReader dr0 = cm0.ExecuteReader();
        //if (dr0.Read())
        //{
        //    _count = Convert.ToInt32(dr0[0]);
        //}
        //else
        //{        }
        //_count++;

        //dr0.Close();

        string path = Server.MapPath("~/Image/");
        path = path+ 
            //+ "I" + _count + ".jpg";
          FileUpload1.FileName;
        FileUpload1.SaveAs(path);
        bool r = sqlinsert("insert into Slide values('" + FileUpload1.FileName + "')");
        if (r)
        {
            Response.Write("<Script>alert('Added')</Script>");
            databind("select * from Slide");
        }
        else
        {
            Response.Write("<Script>alert('Error.Try Later')</Script>");
        }
    }
}