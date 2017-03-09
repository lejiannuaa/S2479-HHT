package com.hola.bs.bean;

import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;

import com.hola.bs.core.UserContainer;
import com.hola.bs.impl.JdbcTemplateUtil;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.property.SQLPropertyUtil;

/**
 * 该对象用于验证用户是否可以登录
 * @author S1608
 *
 */
public class UseVerify {
	
	/**
	 * 用户尚未登录
	 */
	public static int USER_NOT_LOGIN=1;
	
	/**
	 * 相同用户(不同IP)请求登录
	 */
	public static int USER_ALREADY_LOGIN=2;
	
	/**
	 * 用户已登录
	 */
	public static int USER_LOGIN=3;
	
	private ProcessUnit login = null;
	
	@Autowired(required=true)
	private JdbcTemplateUtil jdbcTemplateUtil;
	
	/**
	 * 用户容器
	 */
	@Autowired(required=true)
	private UserContainer userContainer = null;
	
	@Autowired(required = true)
	protected SQLPropertyUtil sqlpropertyUtil;
	
	/**
	 * 判断登录用户的IP是否和是当前门店所在IP段相同
	 * @param ip 用户IP
	 * @return true:是门店员工，false:非门店员工
	 */
	public boolean isSameStore(String ip){
		return true;
	}
	
	/**
	 * 验证用户是否超时
	 * @param name 用户名
	 * @return true:已经超时，false:尚未超时
	 */
	public boolean isTimeout(String name){
//		long time = uc.getUser(name).getLoginTime()-System.currentTimeMillis();
//		int min = (int)time/1000/60;
//		if(Integer.valueOf(bundle.getString("TIMEOUT"))>min){
//			return false;
//		}
		return false;
	}
	
	/**
	 * 验证用户是否已经登录<br>
	 * 不需要通过数据库查询
	 * @param name 用户名
	 * @return true:已经登录，false:尚未登录
	 */
	public boolean isAlreadyLogin(String name){
		
		
		
		if(name=="SOM"||name.equals("SOM")){
			return true;
		}
		
		UserState user = userContainer.getUser(name);
		//如果没有找到这个用户，说明没有登录
		if(user==null){
			return false;
		}
			
		//其余情况认为用户已经登陆
		return true;
		
	}
	
	/**
	 * 验证是否来自相同用户名，不同IP的登录请求<br>
	 * 不需要通过数据库查询
	 * @param name 用户名
	 * @return true:已有不同IP的(相同用户名)用户登录，false:用户尚未登录
	 */
	public int sameUserDiffIP(String name, String ip){
		if(name=="SOM"||name.equals("SOM")){
			return USER_LOGIN;
		}
		UserState user = userContainer.getUser(name);
		
		if(user==null){
			return USER_NOT_LOGIN;
		}
			
		//如果登录用户的IP不等于已存在用户的ip，说明是其他用户尝试登录
		if(!user.getIp().equals(ip)){
			return USER_ALREADY_LOGIN;
		}
			
		//其余情况认为用户已经登陆
		return USER_LOGIN;
		
	}
	
	/**
	 * 验证是否来自相同用户名，不同IP的登录请求<br>
	 * 不需要通过数据库查询
	 * @param name 用户名
	 * @return true:已有不同IP的(相同用户名)用户登录，false:用户尚未登录
	 */
	public boolean sameUserDiffIP2(String name, String ip){
		
		if(name=="SOM"||name.equals("SOM")){
			return true;
		}
		UserState user = userContainer.getUser(name);
		//如果登录用户的IP不等于已存在用户的ip，说明是其他用户尝试登录
		return user.getIp().equals(ip);
	}
	
	/**
	 * 获得设备所在的门店代号
	 * @param ip 设备的ip
	 */
	public String getStoreByIP(String ip){
		
		String sto_ip = (ip.split("\\."))[2];
		List list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(null,"sys1000.02"), new Object[]{sto_ip});
		if(list==null || list.size()<1)
			return null;
		else{
			Map map = (Map)list.get(0);
			return map.get("STO_NO").toString();
		}
	}
	
	/**
	 * 用户密码是否正确
	 * @param password 密码
	 * @return true:密码正确，false:密码错误
	 */
	public String login(Request request){
		return login.process(request);
	}

	public ProcessUnit getLogin() {
		return login;
	}

	public void setLogin(ProcessUnit login) {
		this.login = login;
	}

	public JdbcTemplateUtil getJdbcTemplateUtil() {
		return jdbcTemplateUtil;
	}

	public void setJdbcTemplateUtil(JdbcTemplateUtil jdbcTemplateUtil) {
		this.jdbcTemplateUtil = jdbcTemplateUtil;
	}

	public UserContainer getUserContainer() {
		return userContainer;
	}

	public void setUserContainer(UserContainer userContainer) {
		this.userContainer = userContainer;
	}
	
	public String getToLocation(){
		List list = jdbcTemplateUtil.searchForList("select * from ");
		if(list!=null && list.size()>0 && list.get(0)!=null){
			Map map = (Map)list.get(0);
			return map.get("").toString();
		}
		return null;
	}
	
	public String getFromLocation(){
		List list = jdbcTemplateUtil.searchForList("select * from ");
		if(list!=null && list.size()>0 && list.get(0)!=null){
			Map map = (Map)list.get(0);
			return map.get("").toString();
		}
		return null;
	}

}
