using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

    
    protected void salvaSessione_Click(object sender, EventArgs e)
    {
        Session["Cognome"] = txtCognome.Text;
        Session["Nome"] = TxtNome.Text;
    }


    protected void salvaCokies_Click(object sender, EventArgs e)
    {
        HttpCookie myCookie = new HttpCookie("DataNascita");
        myCookie["DataNascita"] = txtDataNascita.Text;
        myCookie["LuogoNascita"] = txtLocalitaNascita.Text;

        myCookie.Expires = DateTime.Now.AddDays(7);
        Response.Cookies.Add(myCookie);
    }
}