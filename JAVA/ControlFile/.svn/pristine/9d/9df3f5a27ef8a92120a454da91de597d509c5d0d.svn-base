package com.task.common;

import java.io.FileInputStream;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;

import org.jdom.Attribute;
import org.jdom.Document;
import org.jdom.Element;
import org.jdom.input.SAXBuilder;

import com.task.db.DBOperator;

public class PropertysUtil {

	private static PropertysUtil _instenct;
	
	private static Element root ;
	
	private static Element reStartRoot;
	
	private static Element checkRoot;
	
	private DBOperator dbo = new DBOperator();
	
	public static PropertysUtil getInstenct() {
		if (_instenct == null) {
			_instenct = new PropertysUtil();
		}
		return _instenct;
	}
	
	/**
	 * 初始化配置文件
	 *
	 */
	public void initPropertysUtil() {
		
		SAXBuilder sax = new SAXBuilder();
		Document doc=null;
		try{
			String path = "";

			path = this.getClass().getResource("PropertysUtil.class").toString().split("com/task")[0].toString().split("file:/")[1]+"cfg/config.xml";
			System.out.println("path="+path);
			path = "D:/work/Java/workspace/HolaSpace/ControlFile/WebRoot/WEB-INF/classes/com/task/cfg/config.xml";
	
			InputStream is = new FileInputStream(path);
			doc = sax.build(is);	
			root = doc.getRootElement();
		}catch(Exception e){
			e.printStackTrace();
		}
	}
	
	/**
	 * 获得计划任务启动时间、轮询周期
	 * @return
	 */
	public static Hashtable getPlanStartTime() {
		Hashtable ht = new Hashtable();
		try{
			if(root !=null){
				for(int i=0;i<root.getChildren().size();i++){
					Element el = (Element) root.getChildren().get(i);
					if(el.getName().trim().equalsIgnoreCase("StartParam")){
						for(int j=0;j<el.getChildren().size();j++){
							Element child = (Element) el.getChildren().get(j);
							ht.put(child.getName().trim(), child.getText().trim());
						}
					}
				}
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return ht;
	}
	
	/**
	 * 获得数据库连接信息
	 * @return
	 */
	public static Hashtable getConnectInfo(){
		Hashtable ht = new Hashtable();
		try{
			if(root !=null){
				for(int i=0;i<root.getChildren().size();i++){
					Element el = (Element) root.getChildren().get(i);
					if(el.getName().trim().equalsIgnoreCase("Connect")){
						for(int k=0;k<el.getChildren().size();k++){
							Element child = (Element) el.getChildren().get(k);
							ht.put(child.getName().trim(), child.getText().trim());
						}
					}
				}
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return ht;
	}

	public static void main(String[] args) {
		PropertysUtil pu = new PropertysUtil();
		pu.initPropertysUtil();
		
	}
}
