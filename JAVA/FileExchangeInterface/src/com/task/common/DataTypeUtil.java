package com.task.common;

import java.util.Hashtable;
import java.util.List;

import com.task.db.DBOperator;

public class DataTypeUtil {

	private DBOperator dbo = new DBOperator();
	
	/**
	 * �����Ҫͨ��interface�����ݽ������������ͣ������DataHub��ͻ
	 */
	public String getINTFDataType(){
		String typeStr = "";
		Hashtable connectInfo = new Hashtable();
		String url = "jdbc:as400://172.16.251.37;naming=sql;errors=full";
		String driver = "com.ibm.as400.access.AS400JDBCDriver";
		String userName = "JAVAMIS";
		String pwd = "JAVAMIS";
		
		connectInfo.put("Url", url);
		connectInfo.put("Driver", driver);
		connectInfo.put("UserName", userName);
		connectInfo.put("Pwd", pwd);
		
		String sql = "select CHGCOD,SYSCOD,TRGSVR,CHGTYP from mm4r4lib.CHGCTL where SYSCOD like '%HHT% ' or SYSCOD like '%WMS%' or SYSCOD like '%B2B%' or TRGSVR like '%HHT%' or TRGSVR like '%WMS%' or TRGSVR like '%B2B%' or TRGSVR like '%CRM3%' or TRGSVR like '%BOS%'";
		//String sql = "select CHGCOD,SYSCOD,TRGSVR,CHGTYP from mm4r4lib.CHGCTL where SYSCOD like '%HHT% ' or SYSCOD like '%WMS%' or TRGSVR like '%HHT%' or TRGSVR like '%WMS%'";
		List list = dbo.getData(sql, new Object[0], connectInfo);
		for(int i=0;i<list.size();i++){
			Object[] obj = (Object[]) list.get(i);
			if(obj[0]==null){
				obj[0] = "";
			}
			typeStr += "'"+obj[0].toString().trim()+"',";
		}
		if(typeStr.length()>0){
			typeStr = typeStr.substring(0,typeStr.length()-1);
		}
//		System.out.println(typeStr);
		return typeStr;
	}
	
	public static void main(String[] args){
		DataTypeUtil typeUtil = new DataTypeUtil();
		typeUtil.getINTFDataType();
	}
}
