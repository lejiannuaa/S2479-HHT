package com.hola.jda2hht.service;

import java.util.List;

import com.hola.jda2hht.model.ChangeSysBean;

/**
 * 系统表的业务处理
 * 
 * @author issuser
 * 
 */
public interface IChgSysService {
	/**
	 * @remark 获得所有被启用的系统
	 * @file: IChgSysService.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-26
	 * @return
	 */
	List<ChangeSysBean> findAllSys();

}
