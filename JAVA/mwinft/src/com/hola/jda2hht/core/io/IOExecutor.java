package com.hola.jda2hht.core.io;

import java.util.List;
import java.util.Map;

import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.model.ChangeTableBean;
import com.hola.jda2hht.model.ChangeTableDetailBean;
import com.hola.jda2hht.model.CsvBean;
import com.hola.jda2hht.model.JDAChangeInstBean;
import com.hola.jda2hht.model.MergeDto;
import com.hola.jda2hht.service.IJDADataService;

/**
 * 执行数据的接口，他的实现可能会
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-26
 */
public interface IOExecutor {
	/**
	 * IO执行
	 * 
	 * @file: IOExecutor.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 * @param sysBean
	 * @param infoBean
	 * @param inst
	 * @param dataService
	 * @param chgTbl
	 * @param detailMap
	 * @param filePrefix
	 * @param store
	 * @param colName
	 * @return
	 * @throws Exception
	 */
	List<CsvBean> execut(ChangeSysBean sysBean, ChangeInfoBean infoBean,
			JDAChangeInstBean inst, IJDADataService dataService,
			ChangeTableBean chgTbl,
			Map<String, List<ChangeTableDetailBean>> detailMap,
			String filePrefix, String store, String colName) throws Exception;
	
	List<CsvBean> executForMerge(List<MergeDto> dtoList) throws Exception;
	
	
	

}
