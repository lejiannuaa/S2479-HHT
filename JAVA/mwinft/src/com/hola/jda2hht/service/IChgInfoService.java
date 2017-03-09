package com.hola.jda2hht.service;

import java.util.List;

import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.ChangeSysBean;

/**
 * 
 * 交换资料信息的业务
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-26
 */
public interface IChgInfoService {
	/**
	 * 
	 * 根据系统的信息查询该系统下所有的资料信息
	 * 
	 * @file: IChgInfoService.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-26
	 * @param bean
	 * @return
	 */
	List<ChangeInfoBean> getChageBySys(ChangeSysBean bean);
}
