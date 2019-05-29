using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mostraCookie : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string userSettings = "";
        if (Request.Cookies["DataNascita"] != null)
        {
            if (Request.Cookies["DatiNascita"]["DataNascita"] != null)
            { userSettings += Request.Cookies["DatiNascita"]["DataNascita"]; }
            if (Request.Cookies["DatiNascita"]["LocalitaNascita"] != null)
            { userSettings += Request.Cookies["DatiNascita"]["LocalitaNascita"]; }

            Response.Write(userSettings);
        }
    }
}