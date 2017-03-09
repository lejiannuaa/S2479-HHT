package com.hola.jda2hht.core.pipe;

import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.model.JDAChangeInstBean;

/**
 * 
 * @remark 抛转数据的管道
 * @author 唐植超(上海软通)
 * @date 2012-12-26
 */
public interface IPipe {

	/**
	 * 一个批次进行一次抛转动作
	 * 
	 * @file: IPipe.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-27
	 * @param sysBean
	 * @param infoBean
	 * @param inst
	 * @throws Exception
	 */
	public void tran(ChangeSysBean sysBean, ChangeInfoBean infoBean,
			JDAChangeInstBean inst) throws Exception;

}
