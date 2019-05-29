<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    
    <table class="nav-justified">
        <tr>
            <td>
                <asp:Label ID="LBNome" runat="server" Text="Nome"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtNome" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LBCognome" runat="server" Text="Cognome" Placeholder="Cognome"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCognome" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="salvaSessione" runat="server" OnClick="salvaSessione_Click" Text="Salva Sessione" />
            </td>
        </tr>
        <tr>
            <td style="height: 20px">Coockyes data nascita</td>
            <td style="height: 20px">
                <asp:TextBox ID="txtDataNascita" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Coockyes località nascita</td>
            <td>
                <asp:TextBox ID="txtLocalitaNascita" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="salvaCookies" runat="server" OnClick="salvaCokies_Click" Text="Salva Cookies" />
            </td>
        </tr>
        <tr>
            <td><a href="AggiungiSessione.aspx">Aggiungi Sessione</a><br />
                <a href="mostraCookie.aspx">Mostra Cookie</a></td>
            <td>&nbsp;</td>
        </tr>
    </table>
    
</asp:Content>
