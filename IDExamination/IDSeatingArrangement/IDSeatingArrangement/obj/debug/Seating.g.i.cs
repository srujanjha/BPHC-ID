﻿#pragma checksum "..\..\Seating.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "87E0F048E879A09D90F5146888D07E50"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using IDSeatingArrangement;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace IDSeatingArrangement {
    
    
    /// <summary>
    /// Seating
    /// </summary>
    public partial class Seating : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNext;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtStudentsFile;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTimingsFile;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowse;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowse1;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listCourses;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listRooms;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtInvigilators;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTime;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtDate;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGenerate;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtStatus;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\Seating.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdd;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/IDSeatingArrangement;component/seating.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Seating.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnNext = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\Seating.xaml"
            this.btnNext.Click += new System.Windows.RoutedEventHandler(this.btnNext_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtStudentsFile = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtTimingsFile = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.btnBrowse = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\Seating.xaml"
            this.btnBrowse.Click += new System.Windows.RoutedEventHandler(this.btnBrowse_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnBrowse1 = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\Seating.xaml"
            this.btnBrowse1.Click += new System.Windows.RoutedEventHandler(this.btnBrowse_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.listCourses = ((System.Windows.Controls.ListView)(target));
            
            #line 22 "..\..\Seating.xaml"
            this.listCourses.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.listCourses_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.listRooms = ((System.Windows.Controls.ListView)(target));
            return;
            case 8:
            this.txtInvigilators = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.txtTime = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.txtDate = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.btnGenerate = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\Seating.xaml"
            this.btnGenerate.Click += new System.Windows.RoutedEventHandler(this.btnGenerate_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.txtStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 13:
            this.btnAdd = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\Seating.xaml"
            this.btnAdd.Click += new System.Windows.RoutedEventHandler(this.btnAdd_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

