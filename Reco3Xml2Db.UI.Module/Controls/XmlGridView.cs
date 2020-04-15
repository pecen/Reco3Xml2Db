using System;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reco3Xml2Db.UI.Module.Controls.XmlGridView {
  public partial class XmlGridView : UserControl {
    private bool m_bGridViewModeReadError = false;

    public enum VIEW_MODE { XML, TABLE }
    public VIEW_MODE ViewMode {
      get {
        return (webXmlView.Visible ? VIEW_MODE.XML : VIEW_MODE.TABLE);
      }
      set {
        SetViewMode(value);
      }
    }

    private string m_sDataFilePath = string.Empty;
    public string DataFilePath {
      get {
        return m_sDataFilePath;
      }
      set {
        m_sDataFilePath = value;
        LoadDataFile();
      }
    }

    private Stream _xml = null;
    public Stream Xml {
      get => _xml;
      set {
        _xml = value;
        LoadDataFile();
      }
    }

    private int m_nDataSetTableIndex = 0;
    public int DataSetTableIndex {
      get {
        return m_nDataSetTableIndex;
      }
      set {
        SetDataSetTableIndex(value);
      }
    }

    private int m_nDataTableCount = 0;
    public int DataTableCount {
      get { return m_nDataTableCount; }
    }

    public XmlGridView() {
      InitializeComponent();

      SetViewMode(VIEW_MODE.XML);
    }

    private void SetViewMode(VIEW_MODE mode) {
      if (m_bGridViewModeReadError == true) {
        mode = VIEW_MODE.XML;
      }

      if (mode == VIEW_MODE.XML) {
        webXmlView.Visible = true;
        grdTableView.Visible = false;
      }
      else {
        webXmlView.Visible = false;
        grdTableView.Visible = true;
      }
    }

    private void LoadDataFile() {
      m_bGridViewModeReadError = false;

      if (Xml == null && !string.IsNullOrEmpty(m_sDataFilePath)) {
        // use the webbrowser control to automatically parse the file
        webXmlView.Navigate(m_sDataFilePath);

        if ((m_sDataFilePath != string.Empty) && (File.Exists(m_sDataFilePath) == true)) {
          // Creates a DataSet and loads it with the xml content
          try {
            DataSet dsXmlFile = new DataSet();
            dsXmlFile.ReadXml(m_sDataFilePath, XmlReadMode.Auto);
            m_nDataTableCount = dsXmlFile.Tables.Count;

            grdTableView.DataSource = dsXmlFile.Tables[DataSetTableIndex];
          }
          catch {
            m_bGridViewModeReadError = true;
            m_nDataTableCount = 0;
            webXmlView.Navigate(m_sDataFilePath);
            SetViewMode(VIEW_MODE.XML);
          }
        }
        else {
          grdTableView.DataSource = null;
        }
      }
      else {
        webXmlView.DocumentText = new StreamReader(Xml).ReadToEnd();
        Xml.Position = 0;

        try {
          DataSet dsXmlFile = new DataSet();
          dsXmlFile.ReadXml(Xml, XmlReadMode.Auto);
          m_nDataTableCount = dsXmlFile.Tables.Count;

          grdTableView.DataSource = dsXmlFile.Tables[DataSetTableIndex];
        }
        catch {
          m_bGridViewModeReadError = true;
          m_nDataTableCount = 0;
          SetViewMode(VIEW_MODE.XML);
        }
      }
    }

    private void SetDataSetTableIndex(int nTableIndex) {
      if (nTableIndex >= m_nDataTableCount)
        return;

      m_nDataSetTableIndex = nTableIndex;
      LoadDataFile();
    }
  }
}
