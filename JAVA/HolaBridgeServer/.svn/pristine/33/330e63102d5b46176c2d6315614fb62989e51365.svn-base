package com.hola.bs.util;

import java.io.IOException;
import java.io.StringWriter;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.apache.commons.lang.StringUtils;
import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;
import org.dom4j.io.OutputFormat;
import org.dom4j.io.XMLWriter;

public class Root {
    private Config config;
    private List<XmlElement> elements = new ArrayList<XmlElement>();
    public Config getConfig() {
        return config;
    }
    public void addConfig(Config config) {
        this.config = config;
    }
    
    public void addXmlElement(XmlElement xmlElement){
        elements.add(xmlElement);
    }
    public List<XmlElement> getElements() {
        return elements;
    }
    
    public  String asXml() throws SQLException, IOException {
        
        Document doc = DocumentHelper.createDocument();
        Element rootElement = doc.addElement("root");
        
        Element configElement = rootElement.addElement("config");
        Config config = this.getConfig();
        configElement.addElement("type").addText(StringUtils.trimToEmpty(config.getType()));
        configElement.addElement("direction").addText(StringUtils.trimToEmpty(config.getDirection()));
        configElement.addElement("id").addText(StringUtils.trimToEmpty(config.getId()));

        for (XmlElement xmlElement : this.getElements()) {
            Element e = null;
            resultSet2Xml(e, xmlElement,rootElement);
        }
        
        StringWriter out = new StringWriter();
        OutputFormat format = OutputFormat.createPrettyPrint();
        format.setSuppressDeclaration(true);
        //format.setExpandEmptyElements(true);
        XMLWriter writer = new XMLWriter(out, format);
        writer.write(doc);
        writer.close();
        return out.toString();
    }
    
    
    public String asXmlFin() throws SQLException, IOException {
    	
    	 Document doc = DocumentHelper.createDocument();
         Element rootElement = doc.addElement("ROOT");
         Element UPINVINFOElement = rootElement.addElement("UPINVINFO");
         UPINVINFOElement.addAttribute("class", "UPINVINFO");
         Element BASEINFOElement = UPINVINFOElement.addElement("BASEINFO");
         BASEINFOElement.addAttribute("class", "BASEINFO");
         BASEINFOElement.addAttribute("Version", "1.0");
         BASEINFOElement.addElement("NSRSBH").addText("330227793032992");
         BASEINFOElement.addElement("NSRMC").addText("特力屋（上海）商贸有限公司宁波分公司");
         Element FPHZXXJLSElement = UPINVINFOElement.addElement("FPHZXX_JLS");
         FPHZXXJLSElement.addAttribute("class", "FPHZXX_JL;");
         FPHZXXJLSElement.addAttribute("size",String.valueOf(this.getElements().size()));
         for (XmlElement xmlElement : this.getElements()) {
             Element e = null;
             if(xmlElement!=null){
            	 resultSet2XmlFin(e, xmlElement,FPHZXXJLSElement);
             }
             
         }
         
         StringWriter out = new StringWriter();
         out.write("<?xml version='1.0' encoding='UTF-8'?>");  
         OutputFormat format = OutputFormat.createPrettyPrint();
         format.setSuppressDeclaration(true);
         //format.setExpandEmptyElements(true);
         XMLWriter writer = new XMLWriter(out, format);
         writer.write(doc);
         writer.close();
         return out.toString();
    }
    
    private  void resultSet2Xml(Element e, XmlElement xmlElement,Element rootElement) throws SQLException, IOException {
    	List<Map> rs = xmlElement.getResultSet();
//        ResultSetMetaData metadata = rs.getMetaData();
//        int columnCount = metadata.getColumnCount();

        for (Map m:rs) {
        	Set s=m.keySet();
        	Iterator i=s.iterator();
        	e=rootElement.addElement(xmlElement.getTagName());
            while(i.hasNext()) {
            	String key=(String)i.next();
                e.addElement(key).addText(StringUtils.trimToEmpty(String.valueOf( m.get(key))));
            }
        }
    }
    
    private  void resultSet2XmlFin(Element e, XmlElement xmlElement,Element rootElement) throws SQLException, IOException {
    	List<Map> rs = xmlElement.getResultSet();
//        ResultSetMetaData metadata = rs.getMetaData();
//        int columnCount = metadata.getColumnCount();

        for (Map m:rs) {
        	Set s=m.keySet();
        	Iterator i=s.iterator();
        	Element FPHZXXJLElement= rootElement.addElement("FPHZXX_JL");
        	e=FPHZXXJLElement.addElement(xmlElement.getTagName());
        	e.addAttribute("class", "FPHZXX");
            while(i.hasNext()) {
            	String key=(String)i.next();
                e.addElement(key).addText(StringUtils.trimToEmpty(String.valueOf( m.get(key))));
            }
        }
    }
    
    
    /**
     * 解析xml
     * @param xml  字符串xml
     * @param nodeName 需要解析的节点名称
     * @return
     */
    public List<Map> readXML(String xml,String nodeName){
    	List<Map> xmlelement=new ArrayList();
    	 try {
			Document  doc = DocumentHelper.parseText(xml);
			Element rootElt = doc.getRootElement(); // 获取根节点
            System.out.println("根节点：" + rootElt.getName()); // 拿到根节点的名称
            Iterator iter = rootElt.elementIterator("info"); // 获取根节点下的子节点head
            // 遍历head节点
            while (iter.hasNext()) {
                Element recordEle = (Element) iter.next();
                List<Element> l=recordEle.elements();
                Map m=new HashMap();
                for(Element e:l){
                	m.put(e.getName(), e.getText());
                }
                xmlelement.add(m);
              
            }
			
		} catch (DocumentException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} // 将字符串转为XML
    	return xmlelement;
    }
}
