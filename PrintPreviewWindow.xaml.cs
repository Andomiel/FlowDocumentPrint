﻿using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace WpfPrintDemo
{
    partial class PrintPreviewWindow : Window
    {
        private delegate void LoadXpsMethod();
        private readonly Object m_data;
        private readonly FlowDocument m_doc;

        public static FlowDocument LoadDocumentAndRender(string strTmplName, Object data, IDocumentRenderer renderer=null)
        {
            FlowDocument doc = (FlowDocument)Application.LoadComponent(new Uri(strTmplName, UriKind.RelativeOrAbsolute));
            doc.PagePadding = new Thickness(50);
            doc.DataContext = data;
            if (renderer != null)
            {
                renderer.Render(doc, data);
            }
            return doc;
        }

        public PrintPreviewWindow(string strTmplName, Object data, IDocumentRenderer renderer=null)
        {
            InitializeComponent();
            m_data = data;
            m_doc = LoadDocumentAndRender(strTmplName, data, renderer);
            Dispatcher.BeginInvoke(new LoadXpsMethod(LoadXps), DispatcherPriority.ApplicationIdle);
        }

        public void LoadXps()
        {
            //构造一个基于内存的xps document
            MemoryStream ms = new MemoryStream();
            Package package = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
            Uri DocumentUri = new Uri("pack://InMemoryDocument.xps");
            PackageStore.RemovePackage(DocumentUri);
            PackageStore.AddPackage(DocumentUri, package);
            XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Fast, DocumentUri.AbsoluteUri);

            //将flow document写入基于内存的xps document中去
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            writer.Write(((IDocumentPaginatorSource)m_doc).DocumentPaginator);

            //获取这个基于内存的xps document的fixed document
            docViewer.Document = xpsDocument.GetFixedDocumentSequence();

            //关闭基于内存的xps document
            xpsDocument.Close();
        }
    }
}
