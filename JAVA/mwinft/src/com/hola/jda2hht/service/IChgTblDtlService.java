package com.hola.jda2hht.service;

import java.util.List;
import java.util.Map;

import com.hola.jda2hht.model.ChangeTableDetailBean;

/**
 * 交换资料说明表明细 的处理业务
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-26
 */
public interface IChgTblDtlService {
	/**
	 * 获取该源表的所有明细列
	 * 
	 * 对象的结构：{H1=[{item},{item}],H2=[{item},{item}]}
	 * 
	 * @file: IChgTblDtlService.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-26
	 * @param tableId
	 * @return
	 */
	Map<String, List<ChangeTableDetailBean>> getAllTableDetail(String tableId);

	/**
	 * 
	 * 根据tabid和isstr获得一个列的名称
	 * 
	 * @file: IChgTblDtlService.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-4
	 * @param id
	 * @param isstr
	 * @return
	 */
	ChangeTableDetailBean getDetailBean(String id, String isstr);

}
