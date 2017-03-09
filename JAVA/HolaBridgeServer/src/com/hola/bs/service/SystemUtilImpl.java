package com.hola.bs.service;

import java.util.Date;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;

import com.hola.bs.impl.JdbcTemplateUtil;
import com.hola.bs.impl.SystemUtil;
import com.hola.bs.property.SQLPropertyUtil;
import com.hola.bs.util.DateUtils;

public class SystemUtilImpl implements SystemUtil{
	
	@Autowired(required=true)
	private JdbcTemplateUtil jdbcTemplateUtil;
	
	@Autowired(required=true)
	protected SQLPropertyUtil sqlpropertyUtil;
	
	

	public SQLPropertyUtil getSqlpropertyUtil() {
		return sqlpropertyUtil;
	}



	public void setSqlpropertyUtil(SQLPropertyUtil sqlpropertyUtil) {
		this.sqlpropertyUtil = sqlpropertyUtil;
	}



	public JdbcTemplateUtil getJdbcTemplateUtil() {
		return jdbcTemplateUtil;
	}



	public void setJdbcTemplateUtil(JdbcTemplateUtil jdbcTemplateUtil) {
		this.jdbcTemplateUtil = jdbcTemplateUtil;
	}



	public synchronized  String getSysid() {
		// TODO Auto-generated method stub
		return String.valueOf(System.currentTimeMillis());
	}



	public synchronized  String getHHTSysid() {
		// TODO Auto-generated method stub
		return String.valueOf(System.currentTimeMillis());
	}



	public synchronized String getNumId(String store) {

		String numid="";
		String date=DateUtils.DateFormatToString(new Date(), "yyyyMMdd");
		String sql[]=new String[1];
		sql[0]=sqlpropertyUtil.getValue(store,"seq.01");;
		try {
			jdbcTemplateUtil.update(sql, null);
			List<Map> l=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,"seq.02"));
			numid=l.get(0).get("HHTSEQ").toString();
		} catch (Exception e) {
			System.out.println("请初始化HTOSEQ表中数据");
			e.printStackTrace();
		}
		
//		return store+String.valueOf(System.currentTimeMillis());
		return store+date+numid;
	}

}
