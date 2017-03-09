package com.hola.jda2hht.service;

import java.util.List;

import com.hola.jda2hht.model.JDAChangeInstBean;

/**
 * JDA ChgInst表的业务处理
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-27
 */
public interface IJDAChgInstService {
	/**
	 * 根据状态和交换代码查询出所有需要交换的对象
	 * 
	 * @file: IJDAChgInstService.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-27
	 * @param chgCode
	 * @param stuts
	 *            1为需要交换的状态，0为正在交换 ，2为执行完毕
	 * @return
	 */
	List<JDAChangeInstBean> getInstList(String chgCode, String stuts);

	/**
	 * 更新执行的状态 1为需要交换的状态，0为正在交换 ，2为执行完毕
	 * 
	 * @file: IJDAChgInstService.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-27
	 * @param instList
	 * @param stuts
	 */
	void updateStatus(List<JDAChangeInstBean> instList, String stuts);

	/**
	 * 更新执行的状态 1为需要交换的状态，0为正在交换 ，2为执行完毕
	 * 
	 * @file: IJDAChgInstService.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-27
	 * @param inst
	 * @param stuts
	 */
	void updateStatus(JDAChangeInstBean inst, String stuts);
	
	/***
	 * 更新执行的状态 1为需要交换的状态，0为正在交换 ，2为执行完毕
	 * @param inst
	 * @param stuts
	 * @param chgtyp
	 */
	void updateStatus(JDAChangeInstBean inst, String stuts, String chgtyp);

}
