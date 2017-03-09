package com.hola.bs.service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.ResourceBundle;
import java.util.Set;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.property.ConfigPropertyUtil;
import com.hola.bs.util.CommonStaticUtil;
@Transactional(propagation=Propagation.NESTED,rollbackFor=Exception.class)
public class ChginstDao implements IChginstDao{
	private ResourceBundle bundle = ResourceBundle.getBundle("sql");
	
	private Log log = LogFactory.getLog(getClass());

	private String schema = "hhtserver";
	
	@Autowired(required = true)
	private ConfigPropertyUtil configpropertyUtil;
	@Autowired(required = true)
	private JdbcTemplate jdbcTemplate;

	/**
	 * 分解sql语句，判断每个表执行的数量
	 * 
	 * @param sql
	 *            执行语句数组
	 */
	public Map<String, Integer> queryCount(String[] sql, String view,
			Set<String> tables, Object[] ages) {
		// 执行语句数量容器
		Map<String, Integer> map = new HashMap<String, Integer>();

		for (int i = 0; i < sql.length; i++) {
			String s = sql[i].toUpperCase();
			if (s.startsWith("INSERT") || s.startsWith("UPDATE")) {

				// 按照空格分隔SQL语句，取得表名
				String[] cell = s.split(" ");

				// 更新语句的执行数量，不同于insert语句(每一条insert语句只能insert一条数据)，
				// update语句一次性可以执行多条记录，因此需要首先查询出update语句的执行数量
				int updateCount = 1;
				// 根据不同的语句判断下标所在的位置，insert语句的表名在第三个，update语句的表名在第二个
				int index = 2;

				// 如果是update语句，index设定为2，且需要获得update语句的记录数量
				if (cell[0].equals("UPDATE")) {
					index = 1;

					// 需要跳过的参数下标
					// 传入的参数是用于update语句的参数，但是其中包含了需要更新的内容和条件，此处只需要条件，因此需要去掉更新内容
					int skipArgIndex = 0;
					// 去掉更新内容,方法是判断where条件前面有多个问号(参数)
					String[] _str = s.split("WHERE");

					// 通过正则表达式来循环遍历语句中匹配的词
					Pattern p = Pattern.compile("\\?");
					Matcher m = p.matcher(_str[0]);
					while (m.find()) {
						skipArgIndex++;
					}

					ArrayList<Object> list = new ArrayList<Object>();
					Object[] args = (Object[]) ages[i];
					for (int c = skipArgIndex; c < args.length; c++) {
						list.add(args[c]);
					}

					updateCount = getSelectCount(rebuildUpdateToSelect(s), list
							.toArray());
				}

				// 表名
				String tableName = cell[index];

				// 如果存在视图，则使用视图的名称替代表名
				if (view != null) {
					tableName = view;
				}

				// 判断需要执行的表是否存在于配置文件中
				// 如果容器中已存在该表，则表示该表已被操作过，此时需要将原有数字加上updateCount的值(insert、update的记录数量)
				// 如果容器中尚未存在该表，则标示该表尚未被insert过，此时将表存入容器，计数器加上updateCount的值
				if (tables.contains(tableName)) {
					if (map.get(tableName) != null) {
						int count = map.get(tableName) + updateCount;
						map.put(tableName, count);
					} else {
						map.put(tableName, updateCount);
					}
				}
			}
		}

		return map;
	}

	/**
	 * 将更新语句重置为查询语句
	 * 
	 * @param udpate
	 *            更新语句
	 * @return 新的查询语句，用于查询更新语句的执行数量
	 */
	public String rebuildUpdateToSelect(String update) {
		// 按照where条件分割字符串
		String[] cell = update.split("WHERE");
		// 获得需要操作的表名
		String table = cell[0].split(" ")[1];
		// 拼接字符串
		String select = "SELECT COUNT(1) FROM " + schema + "." + table
				+ " WHERE " + cell[1];
		// 返回字符串
		return select;
	}

	/**
	 * 获得查询语句的记录数
	 * 
	 * @param sql
	 *            查询语句
	 * @param ages
	 *            查询参数
	 * @return 查询语句的记录数
	 */
	public int getSelectCount(String sql, Object[] ages) {
		return getJdbcTemplate().queryForInt(sql, ages);
	}

	/**
	 * 通过命令ID(101_01,104_01...)获得对应的交换代码
	 * 
	 * @param commandId
	 *            指令编号
	 * @return
	 */
	public String getChgCode(String commandId) {
		String chgCode = bundle.getString(commandId);
		return chgCode;
	}

	/**
	 * 获得每个视图中数据的数量
	 * 
	 * @param tables
	 * @param instno
	 * @return
	 */
	public Map<String, Integer> queryCount(Set<String> tables, String instno) {
		// 执行语句数量容器
		Map<String, Integer> map = new HashMap<String, Integer>();

		for (String view : tables) {
			String sql = "select count(1) from " + schema + "." + view
					+ " where instno='" + instno + "'";
			map.put(view, jdbcTemplate.queryForInt(sql));
		}

		return map;
	}
	
	/**
	 * 获得每个视图中数据的数量(优化多店共用情况)
	 * 
	 * @param tables
	 * @param instno
	 * @param store
	 * @return
	 */
	public Map<String, Integer> queryCount(Set<String> tables, String instno, String store) {
		// 执行语句数量容器
		Map<String, Integer> map = new HashMap<String, Integer>();

		for (String view : tables) {
			String sql = "select count(1) from " + "hht" + store + "." + view
					+ " where instno='" + instno + "'";
			map.put(view, jdbcTemplate.queryForInt(sql));
		}

		return map;
	}

	public void execute(String commandId, String instno, String[] sqlArr,
			Object[] ages, String store) {

		// 获得交换代码
		String chgCode = getChgCode("hht" + commandId.replace("_", ".")
				+ ".chgcode");

		//this.schema = "hht" + store;

		// 视图名
		String view = null;
		
		String TRGSVR = "JDA";
		
		if(chgCode.equals("SOM_1STCOUNT")){
			TRGSVR = "SOM";
		}else if(chgCode.equals("SOM_2NDCOUNT")){
			TRGSVR = "SOM";
		}else if(chgCode.equals("SOM_AFJQ")){
			TRGSVR = "SOM";
		}else if(chgCode.equals("SOM_EXTCOUNT")){
			TRGSVR = "SOM";
		}else if(chgCode.equals("SOM_LOCLAB")){
			TRGSVR = "SOM";
		}else if(chgCode.equals("SOM_PDJH")){
			TRGSVR = "SOM";
		}
		else if(chgCode.equals("SOM_XDSP")){
			TRGSVR = "SOM";
		}

		// 获得交换代码下配置的所有表
		// String[] tables_ = bundle.getString(chgCode).split(",");

		// 将表存入Set容器中
		// 该set中的存在的表(或者视图)即表示需要数据交换。(也就是需要保存到chginstf中)
		Set<String> tables = new HashSet<String>();
		// for (String s : tables_) {
		// tables.add(s);
		// }

		// 如果存在视图，则取视图名
		if (bundle.containsKey(chgCode + "_VIEW")) {
			view = bundle.getString(chgCode + "_VIEW");
			if (view.contains(",")) {
				String[] views = view.split(",");
				for (String s : views)
					tables.add(s);
			} else {
				tables.add(view);
			}
			// 如果存在视图，则还需要将视图名存入Set中
		}

		// 判断sql数组中操作table的数量
		// Map<String, Integer> map = queryCount(sqlArr, view, tables, ages);
		//Map<String, Integer> map = queryCount(tables, instno);
		Map<String, Integer> map = queryCount(tables, instno, store);

		// 获得容器中的key
		Set<String> mapKey = map.keySet();

		int total = 0;
		System.out
				.println("INSTNO=================================================================>"
						+ instno);
		log
				.info("***********************start write chg info****************************");
		for (String s : mapKey) {
			total += map.get(s);
			String tar = null;
			System.out.println(s);
			if (s.equalsIgnoreCase("htic4f")) {
				tar = "HTIC4F";
			} else if (s.equalsIgnoreCase("htic6f")) {
				tar = "HTIC6F";
			} else if (s.equalsIgnoreCase("htic7f")) {
				tar = "HTIC7F";
			} else if (s.equalsIgnoreCase("jdaid4f")) {
				tar = "HTID4F";
			} else if (s.equalsIgnoreCase("hhtod5")) {
				tar = "HTID5F";
			} else if (s.equalsIgnoreCase("hhtod6")) {
				tar = "HTID6F";
			} else if (s.equalsIgnoreCase("htid7f")) {
				tar = "HTID7F";
			} else if (s.equalsIgnoreCase("hhtod6v2")){
				tar = "HTID6F";
			}else if (s.equalsIgnoreCase("stk_1stcount")){
				tar = "STK_FIRST_COUNT";
			}else if (s.equalsIgnoreCase("stk_2ndcount")){
				tar = "STK_SECOND_COUNT";
			}else if (s.equalsIgnoreCase("stk_ext_count")){
				tar = "STK_EXT_COUNT";
			}else if (s.equalsIgnoreCase("som_extcount")){//新增加的4个chgingst
				tar="SOM_EXTCOUNT";
			}else if (s.equalsIgnoreCase("som_pdjh")){
				tar="SOM_PDJH";
			}else if (s.equalsIgnoreCase("som_afjq")){
				tar="SOM_AFJQ";
			}else if (s.equalsIgnoreCase("som_xdsp")){
				tar="SOM_XDSP";
			}else if (s.equalsIgnoreCase("stk_group")){
				tar="SOM_XDSP";
			}else if (s.equalsIgnoreCase("hhtrcdiff")){
				tar="DMSRCDIFF";
			}else if (s.equalsIgnoreCase("som_group")){
				tar="HTIADF";
			}else if (s.equalsIgnoreCase("hht_c9fhht")){
				tar="HTIC9F";
			}
			String insert2 = "insert into "
					+ "hht" + store
					+ ".CHGINSTF (INSTNO, SRCNAM, SRCCNT, SRCSUM, OTHSUM, TARNAM, TARCNT, TARSUM, TAROTH, SRCLIB, TMPNAM, TMPLIB, TARLIB) ";
			String value2 = " values ('" + instno + "' , '" + s + "', '"
					+ map.get(s) + "', '" + map.get(s) + "', '0', '" + tar
					+ "', '0', '0', '0', '" + "hht" + store + "', '', '', '"
					+ getTarLib() + "')";
			// System.out.println(insert2 + value2);
			log.info(insert2 + value2);
			jdbcTemplate.update(insert2 + value2);
		}

		// 插入目标数据库
		String insert1 = "insert into "
				+ "hht" + store
				+ ".CHGINST (INSTNO, CHGCOD, CHGTYP, SRCDAT, SRCTIM, SRCCNT, SRCUSR, SRCSTS, TRGSVR, FILCNT, CHGDAT, CHGCNT, CHGUSR, CHGSTS, OID, PRTCOD, SYSCOD) ";
		String value1 = " values ('" + instno + "', '" + chgCode + "', 'O', '"
				+ currentData() + "', '" + currentTime() + "', '" + total
				+ "', 'BS', '1', '"+ TRGSVR +"' , '" + tables.size() + "', '"
				+ currentData() + "', '0', 'BS', '1', '', '0', 'MW')";
		// System.out.println("======================="+insert1+value1);
		log.info(insert1 + value1);
		jdbcTemplate.update(insert1 + value1);
		log
				.info("*********************************************************************");
	}

	private String currentData() {
		return CommonStaticUtil.dataFormat("yyyyMMdd");
	}

	private String currentTime() {
		return CommonStaticUtil.dataFormat("hhmmss");
	}

	/**
	 * 获取tarlib
	 * 
	 * @return author: S2138 2013 Feb 19, 2013 4:51:49 PM
	 */
	private String getTarLib() {
		return configpropertyUtil.getValue("Tarlib");
	}

	public ConfigPropertyUtil getConfigpropertyUtil() {
		return configpropertyUtil;
	}

	public void setConfigpropertyUtil(ConfigPropertyUtil configpropertyUtil) {
		this.configpropertyUtil = configpropertyUtil;
	}

	public JdbcTemplate getJdbcTemplate() {
		return jdbcTemplate;
	}

	public void setJdbcTemplate(JdbcTemplate jdbcTemplate) {
		this.jdbcTemplate = jdbcTemplate;
	}
	
	public ResourceBundle getBundle() {
		return bundle;
	}

	public void setBundle(ResourceBundle bundle) {
		this.bundle = bundle;
	}

	public Log getLog() {
		return log;
	}

	public void setLog(Log log) {
		this.log = log;
	}

	public String getSchema() {
		return schema;
	}

	public void setSchema(String schema) {
		this.schema = schema;
	}

}
