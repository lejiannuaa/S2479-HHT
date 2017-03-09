package com.hola.bs.service;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.sql.SQLException;
import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.commons.lang.StringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.scheduling.annotation.Scheduled;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.bean.Response;
import com.hola.bs.bean.UserState;
import com.hola.bs.core.Init;
import com.hola.bs.core.UserContainer;
import com.hola.bs.impl.JdbcTemplateSomUtil;
import com.hola.bs.impl.JdbcTemplateSqlServerUtil;
import com.hola.bs.impl.JdbcTemplateSqlServerUtil2;
import com.hola.bs.impl.JdbcTemplateUtil;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.impl.SystemUtil;
import com.hola.bs.print.rmi.DynamicRmiClient;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.property.ConfigPropertyUtil;
import com.hola.bs.property.SQLPropertyUtil;
import com.hola.bs.util.Config;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.Root;
import com.hola.bs.util.XmlElement;
public abstract class BusinessService implements ProcessUnit {
	public Log log = LogFactory.getLog(BusinessService.class);
	/**
	 * SKU编号的长度
	 */
	public static final int SKU_LENGTH = 9;
	
	//upc长度12
	public static final int UPC_LENGTH_TWELVE = 12;
	
	//upc长度13
	public static final int UPC_LENGTH_THIRTEEN = 13;

	//IPC编号长度
	public static final int IPC_LENGTH = 8;
	
	// 处理结果为成功
	public static String successcode = "0";

	// 处理结果有错误
	public static String errorcode = "1";

	// 用户已经登陆
	public static String userAlreadyLogin = "2";
	
	// 用于对用户在使用系统过程做些警告指示
	public static String warncode = "3";

	@Autowired(required = true)
	protected ConfigPropertyUtil configpropertyUtil;

	@Autowired(required = true)
	@Qualifier("systemUtil")
	protected SystemUtil systemUtil;

	@Autowired(required = true)
	protected SQLPropertyUtil sqlpropertyUtil;

	@Autowired(required = true)
	@Qualifier("jdbcTemplateUtil")
	protected JdbcTemplateUtil jdbcTemplateUtil;
	
	@Autowired(required = true)
	@Qualifier("jdbcTemplateSomUtil")
	protected JdbcTemplateSomUtil jdbcTemplateSomUtil;

	public JdbcTemplateSomUtil getJdbcTemplateSomUtil() {
		return jdbcTemplateSomUtil;
	}

	public void setJdbcTemplateSomUtil(JdbcTemplateSomUtil jdbcTemplateSomUtil) {
		this.jdbcTemplateSomUtil = jdbcTemplateSomUtil;
	}

	@Autowired(required=true)
	@Qualifier("jdbcTemplateSqlServerUtil")
	protected JdbcTemplateSqlServerUtil jdbcTemplateSqlServerUtil;
	
	public JdbcTemplateSqlServerUtil getJdbcTemplateSqlServerUtil() {
		return jdbcTemplateSqlServerUtil;
	}

	public void setJdbcTemplateSqlServerUtil(
			JdbcTemplateSqlServerUtil jdbcTemplateSqlServerUtil) {
		this.jdbcTemplateSqlServerUtil = jdbcTemplateSqlServerUtil;
	}
	/*
	@Autowired(required=true)
	@Qualifier("jdbcTemplateSqlServerUtil2")
	protected JdbcTemplateSqlServerUtil2 jdbcTemplateSqlServerUtil2;
	
	public JdbcTemplateSqlServerUtil2 getJdbcTemplateSqlServerUtil2() {
		return jdbcTemplateSqlServerUtil2;
	}

	public void setJdbcTemplateSqlServerUtil2(
			JdbcTemplateSqlServerUtil2 jdbcTemplateSqlServerUtil2) {
		this.jdbcTemplateSqlServerUtil2 = jdbcTemplateSqlServerUtil2;
	}
	*/

	@Autowired(required = true)
	protected UserContainer userContainer;

	@Autowired(required = true)
	protected IChginstDao chginstDao;

	@Autowired(required = true)
	protected DynamicRmiClient dynamicRmiClient;
	
	// protected Converter
	private UserState user = null;

	public UserContainer getUserContainer() {
		return userContainer;
	}

	public void setUserContainer(UserContainer userContainer) {
		this.userContainer = userContainer;
	}

	public ConfigPropertyUtil getConfigpropertyUtil() {
		return configpropertyUtil;
	}

	public void setConfigpropertyUtil(ConfigPropertyUtil configpropertyUtil) {
		this.configpropertyUtil = configpropertyUtil;
	}

	public SystemUtil getSystemUtil() {
		return systemUtil;
	}

	public void setSystemUtil(SystemUtil systemUtil) {
		this.systemUtil = systemUtil;
	}

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

	/**
	 * 如果传入的是UPC码，则通过查询数据库转换成SKU后返回
	 * 
	 * @return 如果查询到UPC对应的SKU，则返回SKU，否则返回null
	 */
	protected String tranUPCtoSKU(String schema, String upc) {
		String sql = sqlpropertyUtil.getValue(schema, "upc.01");
		List<Map> skuList = jdbcTemplateUtil.searchForList(sql, new Object[] { upc});
        if (skuList != null && skuList.size() > 0) {
			
			Object sku = skuList.get(0).get("hhtsku");
			if (sku != null)
				return sku.toString();
		}
		return null;
	}
	
	protected String tranUPCtoSKUforLengthNinetoEleven(String schema, String upc) {
		String sql = sqlpropertyUtil.getValue(schema, "upc.02");
		List<Map> skuList = jdbcTemplateUtil.searchForList(sql, new Object[] { fullUPCTwelve(upc),fullUPCThirteen(upc) });
		if (skuList != null && skuList.size() > 0) {
			
			Object sku = skuList.get(0).get("hhtsku");
			if (sku != null)
				return sku.toString();
		}
		return null;
	}

	/**
	 * 根据输入的SKU获取商品信息
	 * @param store
	 * @param sku
	 * @return
	 * author: S2139
	 * 2013 Jan 6, 2013 10:24:12 AM
	 */
	protected List getSkuInfo(String store,String sku){
		String finalsku = sku;

		if(sku.length()>UPC_LENGTH_TWELVE){
			finalsku = tranUPCtoSKU(store, sku);
		}else if(sku.length()>SKU_LENGTH && sku.length()<=UPC_LENGTH_TWELVE){
			finalsku = tranUPCtoSKUforLengthNinetoEleven(store, sku);
			
		}else if(sku.length() < IPC_LENGTH){
			finalsku = fullSKU(sku);
		}else if(sku.length() == IPC_LENGTH ){//ipc转换成SKU\
			
			if(sku.substring(0, 1).equals("2")){
				String sql = sqlpropertyUtil.getValue(store, "hht1011.01.04");
				List<Map> skuList = jdbcTemplateUtil.searchForList(sql,new Object[]{sku});
				if(skuList!=null&&skuList.size()>0){
					finalsku = skuList.get(0).get("hhtsku").toString();
				}
			}else{
				String sql = sqlpropertyUtil.getValue(store, "hht1011.01.00");
				List<Map> skuList = jdbcTemplateUtil.searchForList(sql,new Object[]{sku,sku,fullUPCTwelve(sku),fullUPCThirteen(sku)});
				if(skuList!=null&&skuList.size()>0){
					finalsku = skuList.get(0).get("hhtsku").toString();
				}
			}
		}
		List list = null;
		//如果参数SKU为空，则返回空。
		if(finalsku != null && finalsku.length()>0){
			String sql = sqlpropertyUtil.getValue(store, "skuinfo.01");
			list = jdbcTemplateUtil.searchForList(sql, new Object[] {finalsku});
		}
		
		return list;
	}
	/**
	 * 补齐SKU的长度
	 * 
	 * @return
	 */
	protected String fullSKU(String sku) {
		return StringUtils.leftPad(sku, SKU_LENGTH, '0');
	}
	
	/**
	 * 补齐UPC12位长度
	 * 
	 * @return
	 * 
	 */

	protected String fullUPCTwelve(String upc){
		return StringUtils.leftPad(upc, UPC_LENGTH_TWELVE, '0');
	}
	
	
	/**
	 * 补齐UPC13位长度
	 * 
	 * @return
	 * 
	 */

	protected String fullUPCThirteen(String upc){
		return StringUtils.leftPad(upc, UPC_LENGTH_THIRTEEN, '0');
	}

	/**
	 * 解析接收到的请求信息并转换为Map对象
	 * 
	 * @param request
	 */
	protected BusinessBean resolveRequest(Request request) throws Exception{
		BusinessBean bean = new BusinessBean();
		Response response = new Response();
		try {

			response.setCode(BusinessService.successcode);

			response.setDesc(null);

			response.setResponse(request.getParameter("request"));

			user = userContainer.getUser(request.getParameter(Request.USR));

			String op = request.getParameter("op");

			bean.setResponse(response);

			setXmlName(response.getResponse(), op, bean);
		} catch (Exception e) {
			response.setDesc("BusinessService.resolveRequest系统异常 "
					+ e.getMessage());
			response.setCode(BusinessService.errorcode);// 设置返回类型为失败
			log.error("", e);
			throw e;
		} finally {
			bean.setRequest(request);
			bean.setResponse(response);
			bean.setUser(user);
		}
		return bean;

	}

	/**
	 * 设定xml文件名
	 * 
	 * @param request
	 * @param op
	 * @throws Exception
	 */
	protected void setXmlName(String request, String op, BusinessBean bean)
			throws Exception {
		String[] loginCommands = new String[] { "login", "login2","logout" };
		if (op != null && !"".equals(op.trim())) {
			try {
				bean.setXmlname(request + op + ".xml");
			} catch (Exception e) {
				bean.getResponse().setDesc(
						"BusinessService.setXmlName创建文件名失败 " + e.getMessage());
				bean.getResponse().setCode(BusinessService.errorcode);// 设置返回类型为失败
				log.error(e.getMessage());
				throw e;
			}
		} else {
			boolean flag = true;
			for (String s : loginCommands) {
				if (s.equalsIgnoreCase(request)) {// 如果不是登录指令，返回为失败类型，不能创建XML
					flag = false;
					break;
				}
			}
			if (flag) {
				bean.getResponse()
						.setDesc("BusinessService.setXmlName创建文件名失败 ");
				bean.getResponse().setCode(BusinessService.errorcode);// 设置返回类型为失败
				log.error("设定xml文件名失败op===" + op);
			}
			// if(!request.equalsIgnoreCase("login")){//这里区分登录指令与非登录指令
			// bean.getResponse().setDesc("BusinessService.setXmlName创建文件名失败 ");
			// bean.getResponse().setCode(BusinessService.errorcode);//
			// 设置返回类型为失败
			// log.error("设定xml文件名失败op===" + op);
			//
			// }
		}
	}

	/**
	 * 通过配置参数写出xml
	 * 
	 * @param c
	 * @param xml
	 * @return
	 * @throws IOException
	 * @throws SQLException
	 */
	public String toXML(Config c, XmlElement xml[]) throws SQLException,
			IOException {

		Root root = new Root();
		root.addConfig(c);
		if (xml != null) {
			for (XmlElement x : xml) {
				root.addXmlElement(x);
			}
		}
		return root.asXml();

	}

	/**
	 * 将查询出来的记录写入
	 * 
	 * @param c
	 * @param xml
	 * @throws IOException
	 */
	public void writerFile(Config c, XmlElement xml[], BusinessBean bean)
			throws Exception {

		String path = "\\\\"
				+ configpropertyUtil.getValue("HHTPATH")
				+ "\\test\\"
				+ bean.getRequest().getParameter(
						configpropertyUtil.getValue("request")) + "\\";
		// String path = user.getOwnerFilePath();
		if (this.user != null)
			// System.out.println(this.user.getOwnerFilePath());
//			path = "\\\\"
//					+ this.user.getOwnerFilePath()
//					+ "\\"
//					+ bean.getRequest().getParameter(
//							configpropertyUtil.getValue("request")) + "\\";
			
			path="\\\\"+configpropertyUtil.getValue("HHTPATH")
			+ "\\"+bean.getRequest().getParameter(configpropertyUtil.getValue("usr"))
			+ "\\"
			+ bean.getRequest().getParameter(configpropertyUtil.getValue("request"))
			+ "\\";
			
		System.out.println("文件输出:" + path);
		File f = new File(path);

		if (!f.exists())
			CreateDirs(path);

		path += bean.getXmlname();

		FileOutputStream fos = null;
		BufferedWriter out = null;

		String s = toXML(c, xml);

		try {
			fos = new FileOutputStream(path);
			out = new BufferedWriter(new OutputStreamWriter(fos, "UTF-8"));
			out.write(s);
		} finally {
			if (out != null)
				out.close();
			if (fos != null)
				fos.close();
		}

		// fw = new FileWriter(path);
		// fw.write(s, 0, s.length());
		// fw.flush();

	}

	/**
	 * 创建多层文件夹
	 * 
	 * @param path
	 */
	public void CreateDirs(String path) {
		File f = new File(path);
		if (!f.exists())
			f.mkdirs();

	}

	/**
	 * 执行I/U操作，并在执行完后，将数据写入抛转记录表中
	 * 
	 * @param command
	 *            请求指令
	 * @param instno
	 *            暂存编号
	 * @param sql
	 *            需要执行的语句数组
	 * @param ages
	 *            语句的参数
	 */
	protected void update(String command, String instno, String[] sql,
			Object[] ages, String sotre, String guid, String requestValue,
			String memo){
			this.updateForValidation(sql, ages, sotre, guid, requestValue,
							memo);
			// 写入抛转数据
			chginstDao.execute(command, instno, sql, ages, sotre);
			System.out.println("当前本地修改操作总共用时 "
					+ (System.currentTimeMillis() - Init.startMills) + "毫秒");
	}
	
	/**
	 * 改进型的UPDATE方法，增加了储存请求信息的操作
	 * 
	 * @param sql
	 * @param args
	 * @param store
	 * @param guid
	 * @param requestValue
	 *             author: S2139 2012 Oct 31, 2012 5:34:02 PM
	 * @throws Exception 
	 */
	protected void updateForValidation(String[] sql, Object[] args,
			String store, String guid, String requestValue, String memo)
	        {
		if(guid.equals("00000000000000000000000000000000")){
			jdbcTemplateUtil.update(sql, args);
		}else{
			String[] sqlArray = new String[sql.length + 1];
			Object[] o = new Object[args.length + 1];
			System.arraycopy(sql, 0, sqlArray, 0, sql.length);
			System.arraycopy(args, 0, o, 0, args.length);

			String vSql = sqlpropertyUtil.getValue(store,
					"requestValidation.02");
			Object[] vO = new Object[] { guid, requestValue,
					DateUtils.date2StringDate(new Date()), memo };
			sqlArray[sqlArray.length - 1] = vSql;
			o[o.length - 1] = vO;
			jdbcTemplateUtil.update(sqlArray, o);
		}
		
	}
	
	
	protected void newUpdate(String[] sql, Object[] args) {
		jdbcTemplateUtil.update(sql,args);
	}
	
	/**
	 * 判断传入的list中是否存在相同的sku
	 * 
	 * @param list
	 * @param sku
	 * @return
	 */
	protected Map skuExist(List<Map> list, String sku, String skuName) {
		for (int i = 0; i < list.size(); i++) {
			Map m = list.get(i);
			if (m.get(skuName) == null)
				return null;
			if (m.get(skuName).equals(sku))
				return m;
		}
		return null;
	}

	/**
	 * 根据客户端传进来的店铺号码获取打印服务的IP地址，进而生成打印客户端调用对象
	 * 
	 * @param store
	 * @return author: S2139 2012 Sep 25, 2012 10:10:09 AM
	 * @throws Exception
	 */
	protected PrintServer createPrintClient(String store) throws Exception {
		if (Init.printIpMap.containsKey(store)) {
			String ip = Init.printIpMap.get(store);
			return dynamicRmiClient.getRmiPrintClient(ip,
					DynamicRmiClient.REMOTE_PRINT_NAME,
					DynamicRmiClient.REMOTE_PRINT_PORT);
		} else {
			throw new Exception("找不到店铺" + store
					+ "匹配的IP地址，请检查server_ip_address.properties");
		}
	}

	/**
	 * 此方法为备选，jdbcTemplate原生带参数的SQL执行会产生错误，此方法为手动对标识符赋参数值
	 * @param sql
	 * @param args
	 * @return
	 * author: S2139
	 * 2013 Jan 17, 2013 6:05:39 PM
	 */
	protected static String replaceSqlPram(String sql, Object[] args) throws Exception{
		for(int i=0; i<args.length; i++){
			sql = sql.replaceFirst("\\?", "'"+args[i]+"'");
		}
		return sql;
	}
	public DynamicRmiClient getDynamicRmiClient() {
		return dynamicRmiClient;
	}

	public void setDynamicRmiClient(DynamicRmiClient dynamicRmiClient) {
		this.dynamicRmiClient = dynamicRmiClient;
	}

	public UserState getUser() {
		return user;
	}

	public void setUser(UserState user) {
		this.user = user;
	}

	public IChginstDao getChginstDao() {
		return chginstDao;
	}

	public void setChginstDao(IChginstDao chginstDao) {
		this.chginstDao = chginstDao;
	}

}
