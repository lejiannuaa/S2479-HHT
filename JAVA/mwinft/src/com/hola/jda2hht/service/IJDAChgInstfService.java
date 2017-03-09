package com.hola.jda2hht.service;

import java.util.List;

import com.hola.jda2hht.model.JDAChangeInstfBean;

/**
 * JDA的表 chginstf的业务处理类
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-27
 */
public interface IJDAChgInstfService {
	/**
	 * 根据批次号找到需要交换的表
	 * 
	 * @file: IJDAChgInstfService.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-27
	 * @param instno
	 * @return
	 */
	List<JDAChangeInstfBean> getAllIntList(String instno);

}
