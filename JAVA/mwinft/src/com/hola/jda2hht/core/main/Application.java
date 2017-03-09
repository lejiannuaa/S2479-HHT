package com.hola.jda2hht.core.main;

import org.apache.log4j.Logger;

import com.hola.jda2hht.core.executor.JDA2HHTExecutor;
import com.hola.jda2hht.util.ConfigUtil;
import com.hola.jda2hht.util.ThreadPoolUtil;

/**
 * 应用程序入口
 * 
 * @author 唐植超(上海软通)
 * @date 2013-1-8
 */
public class Application {

	static final Logger log = Logger.getLogger(Application.class);

	/**
	 * @param args
	 */
	public static void main(String[] args) {

		// new Thread(new Runnable() {
		//
		// @Override
		// public void run() {
		// 启动程序
		log.info("加载程序");
		load();
		log.info("加载完毕");
		log.info("开始执行");
		execut();
		log.info("执行结束，退出");
		// }
		// }).start();
	}

	/**
	 * 执行
	 */
	protected static void execut() {
		System.out.println("开始执行程序");
		try {
			// 启动线程池
			log.info("启动线程池");
			ThreadPoolUtil.initialize();
			// 获得执行器并启动执行器
			log.info("启动执行器");
			((JDA2HHTExecutor) ConfigUtil.getBean("jda2hhtExecutor"))
					.executor();
		} catch (Exception e) {
			log.error("执行失败" + e.getMessage(),e);
			e.printStackTrace();
		}
		log.info("程序退出");
	}

	/**
	 * 加载
	 */
	protected static void load() {
		log.info("加载spring文件");
		ConfigUtil.loadConfig();
	}

}
