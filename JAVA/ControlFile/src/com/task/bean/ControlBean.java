package com.task.bean;

import java.util.Hashtable;
import java.util.List;

import org.apache.log4j.Logger;

import com.task.common.PropertysUtil;
import com.task.db.DBOperator;

public class ControlBean{
	
	private DBOperator dbo = new DBOperator();
	
	private static Logger log = Logger.getLogger(ControlBean.class);
	
	public Hashtable getInvokeList(){
		Hashtable ht = new Hashtable();
		try{
			String sql = "select CHGCODE,STATUS,CHGDESC,SYSCODE,CHGTYPE,COMMAND,CHGFRQ,CHGVALUE,to_char(CHGSTART,'yyyy-mm-dd hh24:mi:ss'),to_char(CHGEND,'yyyy-mm-dd hh24:mi:ss'),to_char(NEXTDATE,'yyyy-mm-dd hh24:mi:ss'),CLNFRQ,CLNVALUE,to_char(CLNDATE,'yyyy-mm-dd hh24:mi:ss'),FILECNT,TRGSERVER,OWNER,CREATER,to_char(CREATEDATE,'yyyy-mm-dd hh24:mi:ss') from CHGCTL where STATUS<>?";
			Object[] obj = new Object[1];
			obj[0] = "D";
			
			List list = dbo.getData(sql, obj, PropertysUtil.getConnectInfo());
			for(int i=0;i<list.size();i++){
				Object[] rsObj = (Object[]) list.get(i);
				Hashtable infoHT = new Hashtable();
				
				infoHT.put("COMMAND", rsObj[5].toString().trim());//启动命令		COMMAND
				infoHT.put("CHGFRQ", rsObj[6].toString().trim());//交换频率			CHGFRQ		D:天 H:小时 M:分钟 S:秒
				infoHT.put("CHGVALUE", rsObj[7].toString().trim());//交换频率值		CHGVALUE
				infoHT.put("CHGSTART", rsObj[8].toString().trim());//交换开始时间	CHGSTART
				infoHT.put("CHGEND", rsObj[9].toString().trim());//交换截止时间 		CHGEND
				if(rsObj[10]!=null){
					infoHT.put("NEXTDATE", rsObj[10].toString().trim());//下次交换时间 	NEXTDATE
				}else{
					infoHT.put("NEXTDATE", "");//下次交换时间 	NEXTDATE
				}
				if(rsObj[11]!=null){
					infoHT.put("CLNFRQ", rsObj[11].toString().trim());//清除频率		CLNFRQ
				}else{
					infoHT.put("CLNFRQ", "");//清除频率		CLNFRQ
				}
				if(rsObj[12]!=null){
					infoHT.put("CLNVALUE", rsObj[12].toString().trim());//清除频率值	CLNVALUE
				}else{
					infoHT.put("CLNVALUE", "");//清除频率值	CLNVALUE
				}
				ht.put(rsObj[0].toString().trim(), infoHT);
			}
		}catch(Exception e){
			e.printStackTrace();
		}
		return ht;
	}
	

	
	public static void main(String[] args){
		PropertysUtil pu = new PropertysUtil();
		pu.initPropertysUtil();
	}
	
}
