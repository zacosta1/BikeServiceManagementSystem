using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Contact : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void SendMessageButton_Click(object sender, EventArgs e)
    {
        NameTextBox.Text = null;
        EmailTextBox.Text = null;
        SubjectTextBox.Text = null;
        messageTextArea.InnerText = null;

    }
}