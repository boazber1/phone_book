﻿using PhoneBook.ViewModels.Contacts.ViewModel;
using PhoneBook.ViewModels.Forum.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneBook.ViewModels.Contacts.View
{
    /// <summary>
    /// Interaction logic for ContactsView.xaml
    /// </summary>
    public partial class ContactsView : Window
    {
        public ContactsView()
        {
            InitializeComponent();
            var viewModel  = new ContactsViewModel();
            viewModel.OnNewContactClicked += OpenForumOnAddNewcontact;
            //viewModel.OnEditContactClicked += OpenForumOnEditNewcontact;
            DataContext = viewModel;          
        }
     
        public void OpenForumOnAddNewcontact(object sender, EventArgs e)
        {
            ForumView forumView = new ForumView();
            forumView.ShowDialog();
           
        }

        public void OpenForumOnEditNewcontact(object sender, EventArgs e)
        {
            ForumView forumView = new ForumView();
            forumView.ShowDialog();
        }
    }
}