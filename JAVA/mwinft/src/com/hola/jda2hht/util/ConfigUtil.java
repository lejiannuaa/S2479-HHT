package com.hola.jda2hht.util;

import java.io.File;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.ResourceBundle;

import javax.sql.DataSource;

import org.apache.log4j.Logger;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.jda2hht.core.med.FTPTerminalImpl;
import com.hola.jda2hht.core.med.ITerminal;
import com.hola.jda2hht.core.med.MQTerminalImpl;
import com.hola.jda2hht.core.pipe.IPipe;
import com.hola.jda2hht.dao.DB2DataSouce;
import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.ChangeSysBean;

/**
 * 
 * @remark 系统配置类
 * @author 唐植超(上海软通)
 * @date 2012-12-26
 */
public class ConfigUtil {
	private static final Logger log = Logger.getLogger(ConfigUtil.class);
	/**
	 * 缓存数据
	 */
	private static Map<Object, Object> configMap = new HashMap<Object, Object>();
	
	
	// 日期格式 用于csv文件名，保存三位毫秒数
	private static SimpleDateFormat csvSdf = new SimpleDateFormat(
			"yyyyMMddHHmmssSSS");
	// csv文件的目录名，根据日期产生，这样可以每天产生一个目录，便于管理
	private static SimpleDateFormat dirSdf = new SimpleDateFormat("_yyyy_MM_dd");
	private static ResourceBundle rb;

	// Spring 你懂的……
	private static ApplicationContext ctx;
	
	private static int create_file_index = 0;
	
	private static String getCreFileIndex()
	{
		if(++ create_file_index >= 999)
			create_file_index = 0;
		String totle = "" + create_file_index;
		if(create_file_index < 10)
			totle = "00" + create_file_index;
		else if(create_file_index < 100)
			totle = "0" + create_file_index;
		return totle;
	}

	/**
	 * 加载配置
	 */
	public static void loadConfig() {
		// 加载配置文件
		log.info("加载spring");
		ctx = new ClassPathXmlApplicationContext("applicationContext.xml");
		// 配置文件
		log.info("加载配置文件");
		rb = ResourceBundle.getBundle("config");
		// 一下信息我主张就这样放着，当然你可以放到配置文件里面
		log.info("定义常量");
		// 分隔符
		configMap.put("TAB", "\t");
		// 换行符
		configMap.put("LN", "\n");
		// 结尾的字符
		configMap.put("END", "END");
		// csv文件目录
		configMap.put("csvDir", rb.getString("csv_dir"));
	}

	/**
	 * 获得配置信息
	 * 
	 * @file: ConfigUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-8
	 * @param key
	 * @return
	 */
	public static String getConfig(String key) {
		return rb.getString(key);
	}

	/**
	 * 放值
	 * 
	 * @param key
	 * @param value
	 */
	public static void setAttribute(String key, String value) {
		configMap.put(key, value);
	}

	/**
	 * 取值
	 * 
	 * @param key
	 * @return
	 */
	public static Object getAttribute(String key) {
		return configMap.get(key);
	}

	/**
	 * 获取spring对象
	 * 
	 * @param name
	 * @return
	 */
	public static Object getBean(String name) {
		return ctx.getBean(name);
	}

	/**
	 * 创建jdbcTemplet
	 * 
	 * @file: ConfigUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-27
	 * @param sys
	 * @return
	 * @throws Exception
	 */
	public static JdbcTemplate createTemlet(ChangeSysBean sys) throws Exception {
		String key = sys.getDbdriver() + ":" + sys.getDburl() + ":"
				+ sys.getUsername() + ":" + sys.getPassword();
		if (configMap.containsKey(key)) {
			return (JdbcTemplate) configMap.get(key);
		}
		DataSource ds = new DB2DataSouce(sys.getDbdriver(), sys.getDburl(),
				sys.getUsername(), sys.getPassword());
		// 创建Templet
		JdbcTemplate jdbcTemplet = new JdbcTemplate();
		jdbcTemplet.setDataSource(ds);
		// 缓存
		configMap.put(key, jdbcTemplet);
		return jdbcTemplet;
		// 下面这个现成的也可以用哦，只是我觉得这样应该会让性能变差，
		// ComboPooledDataSource ds;
		// ds = new ComboPooledDataSource();
		// // 驱动类
		// ds.setDriverClass(sys.getDbdriver());
		// // 链接地址
		// ds.setJdbcUrl(sys.getDburl());
		// // 数据库用户
		// ds.setUser(sys.getUsername());
		// // 用户密码
		// ds.setPassword(sys.getPassword());
		// // 指定连接数据库连接池的最大连接数
		// ds.setMaxPoolSize(30);
		// // 指定连接数据库连接池的最小连接数
		// ds.setMinPoolSize(1);
		// // 指定连接数据库连接池的初始化连接数
		// ds.setInitialPoolSize(1);
		// // 每30秒检查所有连接池中的空闲连接。Default: 0
		// ds.setIdleConnectionTestPeriod(30);
		// // 最大空闲时间,30秒内未使用则连接被丢弃。若为0则永不丢弃。Default: 0
		// ds.setMaxIdleTime(30);
		// //
		// c3p0是异步操作的，缓慢的JDBC操作通过帮助进程完成。扩展这些操作可以有效的提升性能通过多线程实现多个操作同时被执行。Default:3
		// ds.setNumHelperThreads(5);
		// // 定义在从数据库获取新连接失败后重复尝试的次数。Default: 30
		// ds.setAcquireRetryAttempts(30);
		// // 当连接池中的连接耗尽的时候c3p0一次同时获取的连接数。Default: 3
		// ds.setAcquireIncrement(10);
		// // 自动提交
		// ds.setAutoCommitOnClose(true);
	}

	/**
	 * 
	 * @remark 根据输出的数据格式不同，创建不同的管道
	 * @file: ConfigUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-26
	 * @param chgType
	 * @return
	 * @throws Exception
	 */
	public static IPipe getPipe(String chgType) throws Exception {
		if ("1".equals(chgType)) {
			// 创建csv的管道
			return (IPipe) ctx.getBean("csvPipe");
		}
		// 其他的实现目前没有
		throw new Exception("目前没有其管道实现 ，管道类型：" + chgType);
	}

	/**
	 * 输出的终端
	 * 
	 * @file: ConfigUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-27
	 * @param chgmed
	 * @return
	 */
	public static ITerminal getTerminal(String chgmed) throws Exception {
		// 这个地方我本来想通过spring配置实现的，但是因为他是通过
		// 数据库的字段来动态获取的，数据的的这个字段有只有一个字符，
		// 我就通过懒加载的方式实现，其实我也可以配置到spring中，
		// 只是这样会更直观，在效果上面差别不大
		if (configMap.containsKey(chgmed)) {
			return (ITerminal) configMap.get(chgmed);
		}
		// ftp的实现方式
		if ("F".equals(chgmed)) {
			ITerminal itm = new FTPTerminalImpl();
			configMap.put(chgmed, itm);
			return itm;
		}
		// MQ的实现方式
		if ("M".equals(chgmed)) {
			ITerminal itm = new MQTerminalImpl();
			configMap.put(chgmed, itm);
			return itm;
		}
		//
		throw new Exception("配置的媒体终端不正确，需要检查数据库表中的配置");
	}

	/**
	 * 生成csv路径的规则
	 * 
	 * @file: ConfigUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-4
	 * @param prefix
	 * @return
	 */
	public synchronized static String createCSVPath(String prefix) {
		String dir = configMap.get("csvDir") + dirSdf.format(new Date())
				+ File.separator;
		File file = new File(dir);
		if (!file.exists()) {
			// 每天会新建一个目录
			file.mkdirs();
		}
		return dir + prefix + "_" + csvSdf.format(new Date()) + getCreFileIndex() + ".csv";
	}

	/**
	 * 分隔符的获取
	 * 
	 * @file: ConfigUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-5
	 * @param seqcode
	 * @return
	 */
	public static Object getSeparator(String seqcode) {
		// 这是个小小的插曲，也就是说，如果不是Tab是其他分隔符
		// 我就照单返回，这只是符和当前的需求
		if ("TAB".equals(seqcode)) {
			return "\t";
		}
		return seqcode;
	}

	/**
	 * 创建MQ对象
	 * 
	 * @file: ConfigUtil.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 * @param infoBean
	 * @return
	 */
	public synchronized static JMSSender createMQ(ChangeInfoBean infoBean) {
		if (configMap.containsKey(infoBean)) {
			return (JMSSender) configMap.get(infoBean);
		}
		int transportType = Integer.parseInt(ConfigUtil
				.getConfig("transportType"));
		String host = infoBean.getMqip();
		int ccsid = Integer.parseInt(infoBean.getQccsid());
		String queueManagerName = infoBean.getQmgname();
		int port = Integer.parseInt(infoBean.getQmgport());
		long receiveTimeout = Integer.parseInt(ConfigUtil
				.getConfig("receiveTimeout"));
		String userName = infoBean.getMqusername();
		String passWord = infoBean.getMqpwd();
		// 目前没有更好的实例化方法
		JMSSender mq = new JMSSender(transportType, host, ccsid,
				queueManagerName, port, receiveTimeout, userName, passWord);
		configMap.put(infoBean, mq);
		return mq;
	}
}
