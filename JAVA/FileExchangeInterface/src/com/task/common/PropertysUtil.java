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
//			System.out.println("path="+path);
//			path = "D:/work/Java/workspace/HolaSpace/FileExchangeInterface/WebRoot/WEB-INF/classes/com/task/cfg/config.xml";
	
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
	
	/**
	 * 获得所有需要数据交换的数据库连接信息
	 * 
	 */
	public void getDBConnInfo(){
		try{
			//for SAP、MSG、INTC、JDA
			String sql = "select CODE,URL,DRIVER,USERNAME,PASSWORD,ISREAD,MEMO,ISWRITE,INSTLIB from interface.DBCONNINFO where TYPE is null";
			
			//for CRM
//			String sql = "select CODE,URL,DRIVER,USERNAME,PASSWORD,ISREAD,MEMO,ISWRITE,INSTLIB from interface.DBCONNINFO where USESYS ='CRM' or USESYS ='crm'";
			
			
			List list = dbo.getData(sql, new Object[0], this.getConnectInfo());
			for(int i=0;i<list.size();i++){
				Object[] obj = (Object[]) list.get(i);
				Hashtable ht = new Hashtable();
				System.out.println("code="+obj[0].toString().trim());
				ht.put("Code", obj[0].toString().trim());
				ht.put("Url", obj[1].toString().trim());
				ht.put("Driver", obj[2].toString().trim());
				ht.put("UserName", obj[3].toString().trim());
				ht.put("Pwd", obj[4].toString().trim());
				if(obj[5]==null){
					obj[5] = "";
				}
				ht.put("isRead", obj[5].toString().trim());
				
				if(obj[6]==null){
					obj[6] = "";
				}
				ht.put("Memo", obj[6].toString().trim());
				
				if(obj[7]==null){
					obj[7] = "";
				}
				ht.put("isWrite", obj[7].toString().trim());
				
				if(obj[8]==null){
					obj[8] = "";
				}
				ht.put("InstLib", obj[8].toString().trim());
				
				DataPool.connInfoList.add(ht);
				DataPool.connInfoHT.put(obj[0].toString().trim(), ht);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
	}

	public static void main(String[] args) {
		PropertysUtil pu = new PropertysUtil();
		pu.initPropertysUtil();
		pu.getDBConnInfo();
	}
}
