package com.hola.bs.property;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.Properties;

import org.apache.commons.lang.StringUtils;


public class SQLPropertyUtil {
	 Properties props=new Properties();
	 public SQLPropertyUtil(){
		
		try {
			File f=new File(this.getClass().getResource("/").getFile()+"/sql.properties");
//			File f = new File(System.getProperty("user.dir")+"\\sql.properties");
			System.out.println(f.exists());
			FileInputStream fis  = new FileInputStream(f.getPath());;
//			try {
//				fis = new FileInputStream(str);
//			props.load(System.class.getClass().getResourceAsStream(System.class.getClass().getResource("/")+"/com/hola/contract/resources/config.properties"));
			props.load(fis);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	/**
	 * 获得配置文件的语句
	 * @param schema 执行的SQL语句需要对应的数据库名
	 * @param id 配置文件中SQL对应的id 
	 * @return
	 */
	public  String getValue(String schema, String id){
		String sql = props.getProperty(id);
		if(schema!=null)
			sql = StringUtils.replace(sql, "schema", "hht"+schema);
		return sql;
	}
	
	/**
	 * 获取SQL配置文件信息
	 * @param key
	 * @return
	 * author: S2139
	 * 2013 Jan 24, 2013 1:41:54 PM
	 */
	public String getString(String key){
		return props.getProperty(key);
	}
	
	/**
	 * 确认SQL配置文件里是否有指定KEY
	 * @param key
	 * @return
	 * author: S2139
	 * 2013 Jan 24, 2013 2:02:19 PM
	 */
	public boolean containsKey(String key){
		return props.containsKey(key);
	}
	public static void main(String[] args){
//		System.out.println((new SQLPropertyUtil()).getValue("oracle.url"));
	}
}
