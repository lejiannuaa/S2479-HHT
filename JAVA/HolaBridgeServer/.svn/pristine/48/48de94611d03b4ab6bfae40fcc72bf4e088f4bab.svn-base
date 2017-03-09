package com.hola.bs.bean;

import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;

import com.hola.bs.impl.JdbcTemplateUtil;
import com.hola.bs.property.ConfigPropertyUtil;
import com.hola.bs.property.SQLPropertyUtil;
import com.hola.bs.util.CommonStaticUtil;

/**
 * 验证请求的业务
 * @author S2139
 * 2012 Oct 30, 2012 6:20:23 PM 
 */
public class RequestValidation {
	
	@Autowired(required=true)
	private JdbcTemplateUtil jdbcTemplateUtil;
	
	@Autowired(required = true)
	protected SQLPropertyUtil sqlpropertyUtil;
	
	@Autowired(required = true)
	protected ConfigPropertyUtil configpropertyUtil;
	/**
	 * 验证是否是重复提交的请求，方法是，在请求时获取GUID，在数据库中做判断
	 * @param guid
	 * @return
	 * author: S2139
	 * 2012 Oct 30, 2012 6:13:53 PM
	 */
	public List<Map> validateRequest(String guid,String schema){
//		boolean flag = false;
		String sql = sqlpropertyUtil.getValue(schema, "requestValidation.01");
		List<Map> list = jdbcTemplateUtil.searchForList(sql, new Object[]{guid});
		
		return list;
	}

	/**
	 * 将新的请求插入到数据表里
	 * @param guid
	 * @param receivedRequest
	 * @return
	 * author: S2139
	 * 2012 Oct 30, 2012 6:15:26 PM
	 * @throws Exception 
	 */
	public int insertNewRequest(String guid, String receivedRequest) throws Exception{
		String sql = sqlpropertyUtil.getValue("hht"+configpropertyUtil.getValue("schema"), "requestValidation.02");
		Object[] o=new Object[]{guid,receivedRequest,CommonStaticUtil.getDate()};
		return jdbcTemplateUtil.update(new String[]{sql}, new Object[]{o});
	}

}
