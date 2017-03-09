package com.hola.jda2hht.core.io.impl;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;

import org.apache.log4j.Logger;

import com.hola.jda2hht.core.io.IOExecutor;
import com.hola.jda2hht.model.BatMappingBean;
import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.model.ChangeTableBean;
import com.hola.jda2hht.model.ChangeTableDetailBean;
import com.hola.jda2hht.model.CsvBean;
import com.hola.jda2hht.model.JDAChangeInstBean;
import com.hola.jda2hht.model.LayerBean;
import com.hola.jda2hht.model.MergeDto;
import com.hola.jda2hht.service.IBatMappingService;
import com.hola.jda2hht.service.IJDADataService;
import com.hola.jda2hht.util.ConfigUtil;

/**
 * csv的io操作实现
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-28
 */
public class CSVIOExecutorImpl implements IOExecutor {
	private static final Logger log = Logger.getLogger(CSVIOExecutorImpl.class);

	private IBatMappingService batMappingService;

	public IBatMappingService getBatMappingService() {
		return batMappingService;
	}

	public void setBatMappingService(IBatMappingService batMappingService) {
		this.batMappingService = batMappingService;
	}

	/**
	 * 根据表空间的名称，表名，和列名，封装sql，
	 * 
	 * 获得数据，并封装成csv 同时输出文件
	 * 
	 * @file: CSVPipe.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-28
	 * @param sysBean
	 * @param infoBean
	 * @param tableName
	 * @param detailMap
	 * @param inst
	 * @param dataService
	 * @param colName
	 * @param store
	 * @return
	 */
	
	
	
	private List<CsvBean> out(ChangeSysBean sysBean, ChangeInfoBean infoBean,
			String tableName,
			Map<String, List<ChangeTableDetailBean>> detailMap,
			JDAChangeInstBean inst, IJDADataService dataService,
			String filePrefix, String store, String colName) throws Exception {
		log.info("创建csv对象 空间名：" + sysBean.getDbschema() + " 表名：" + tableName);
		String separator = ConfigUtil.getSeparator(infoBean.getSeqcode())
				.toString();
		String ln = ConfigUtil.getAttribute("LN").toString();
		// 不需要分批
		if ("N".equals(infoBean.getIsbat())) {
			List<CsvBean> list = new ArrayList<CsvBean>();
			CsvBean bean = notBat(sysBean, infoBean, tableName, detailMap,
					inst, dataService, filePrefix, store, colName, separator,
					ln);
			list.add(bean);
			return list;
		}
		// 这里需要分批
		if ("Y".equals(infoBean.getIsbat())) {
			return isBat(sysBean, infoBean, tableName, detailMap, inst,
					dataService, filePrefix, store, colName, separator, ln);
		}
		return null;
	}
	
	
	private List<CsvBean> outForMerge(List<MergeDto> dtoList) throws Exception {
		log.info("创建csv对象 空间名：" + dtoList.get(0).getChangeSysBean().getDbschema() + " 表名：" + dtoList.get(0).getChgTbl().getSrctblname());

		String ln = ConfigUtil.getAttribute("LN").toString();
		// 不需要分批
		if ("N".equals(dtoList.get(0).getChangeInfoBean().getIsbat())) {
			List<CsvBean> list = new ArrayList<CsvBean>();
			CsvBean bean = notBatForMerge(dtoList,
					ln);
			list.add(bean);
			return list;
		}
		// 这里需要分批
		if ("Y".equals(dtoList.get(0).getChangeInfoBean().getIsbat())) {
			return isBatForMerge(dtoList, ln);
		}
		return null;
	}

	/**
	 * 分批
	 * 
	 * @file: CSVIOExecutorImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 * @param sysBean
	 * @param infoBean
	 * @param tableName
	 * @param detailMap
	 * @param inst
	 * @param dataService
	 * @param filePrefix
	 * @param store
	 * @param colName
	 * @param separator
	 * @param ln
	 * @return
	 * @throws Exception
	 */
	private List<CsvBean> isBat(ChangeSysBean sysBean, ChangeInfoBean infoBean,
			String tableName,
			Map<String, List<ChangeTableDetailBean>> detailMap,
			JDAChangeInstBean inst, IJDADataService dataService,
			String filePrefix, String store, String colName, String separator,
			String ln) throws Exception {
		log.info("需要分批");
		CsvBean bean = null;
		String sql = null;
		List<CsvBean> list = new ArrayList<CsvBean>();
		String fileName;
		// 根据每个层 查询出对应的东西
		for (Entry<String, List<ChangeTableDetailBean>> entry : detailMap
				.entrySet()) {

			String filePath = ConfigUtil.createCSVPath(filePrefix);
			File f = new File(filePath);
			log.info(inst + "----" + filePath);
			fileName = f.getName();
			bean = new CsvBean(separator, ln, f);

			sql = createDataCountSQL(sysBean.getDbschema(), tableName,
					entry.getKey(), inst, store, colName);
			int count = dataService.searchScanlar(sql);
			log.info("构建的sql:" + sql);
			LayerBean laybean = createLayBean(infoBean, store, f, entry, count);
			int pageSize = 6000;
			// 查询最大的rownum并保持
			String table = sysBean.getDbschema() + "." + tableName;
			String colPamar = "";
			if (colName != null && store != null) {
				colPamar = " and " + colName + "='" + store + "' ";
			}
			int maxRowNum = 0;
			// as400数据库不同的地方，就是这个鬼一样的分页，需要记录这个
			if(sysBean.getDbtype().equals(ChangeSysBean.DB_ORACLE))
			{
				
				maxRowNum = dataService.searchScanlar("SELECT MAX(rownum)+1 FROM " + table + " where INSTNO='"
						+ inst.getInstno() + "' " + colPamar);
			}else if(sysBean.getDbtype().equals(ChangeSysBean.DB_SQLSERVER)){
				
				maxRowNum = dataService.searchScanlar("SELECT count(*) FROM " + table + " where INSTNO='"
						+ inst.getInstno() + "' " + colPamar);
			} else
			{
				String maxSql = "SELECT MAX(RRN(" + table
						+ "))+1 FROM " + table + " where INSTNO='"
						+ inst.getInstno() + "' " + colPamar;
				log.info("maxRowNum SQL:" + maxSql);
				maxRowNum = dataService.searchScanlar(maxSql);

			}
			
			bean.writeLayHead(laybean);
			// 每1000条进行一次查询
			log.info("翻页查找:" + maxRowNum);
			int laySize = 0;
			for (int i = 0; i < count; i += pageSize) {
				// 拼接翻页
				sql = createPageSql(table, entry.getValue(), inst.getInstno(),
						maxRowNum, pageSize, store, colName , sysBean.getDbtype());
				log.info("翻页查找：" + sql);
//				List<String[]> dataList = dataService.searchData(sql, null,
//						entry.getValue());
				List<String[]> dataList = dataService.searchData(sql, null,
						entry.getValue(),table);
				bean.appendLayData(dataList);
				// 记录最大页数
				maxRowNum -= pageSize;
				log.info("翻页查找:" + maxRowNum);
				laySize += dataList.size();
				// 开始分文件
				if (laySize >= infoBean.getBatcnt()) {
					laybean.setSize(laySize);
					bean.wrirteLayFoot(laybean);
					// 添加一个层
					bean.getLayerList().add(laybean);
					bean.writeEnd();
					// 将现有的对象添加到集合中
					list.add(bean);
					// 添加一个映射记录
					BatMappingBean bmb = new BatMappingBean();
					bmb.setChgcode(infoBean.getChgcode());
					bmb.setInstno(inst.getInstno());
					bmb.setFilname(fileName);
					this.batMappingService.addMapping(bmb);
					laySize = 0;
					// 重新构建新的对象
					filePath = ConfigUtil.createCSVPath(filePrefix);
					log.info("换文件：" + filePath);
					f = new File(filePath);
					fileName = f.getName();
					bean = new CsvBean(separator, ln, f);
					laybean = createLayBean(infoBean, store, f, entry, count);
					// 层的头部
					bean.writeLayHead(laybean);
				}
			}
			// 如果最后一次有余数，肯定需要在查询一次
			if (count > pageSize && count % pageSize > 0) {
				// 拼接翻页
				sql = createPageSql(table, entry.getValue(), inst.getInstno(),
						maxRowNum, pageSize, store, colName, sysBean.getDbtype());
				log.info("翻页查找剩下的：" + sql);
//				List<String[]> dataList = dataService.searchData(sql, null,
//						entry.getValue());
				List<String[]> dataList = dataService.searchData(sql, null,
						entry.getValue(),table);
				bean.appendLayData(dataList);
				log.info("翻页查找剩下的:" + maxRowNum);
				laySize += dataList.size();
			}
			laybean.setSize(laySize);
			bean.wrirteLayFoot(laybean);
			// 添加一个层
			bean.getLayerList().add(laybean);
			list.add(bean);
			// 添加一个映射记录
			BatMappingBean bmb = new BatMappingBean();
			bmb.setChgcode(infoBean.getChgcode());
			bmb.setInstno(inst.getInstno());
			bmb.setFilname(fileName);
			this.batMappingService.addMapping(bmb);
		}
		return list;
	}

	/**
	 * 不分批
	 * 
	 * @file: CSVIOExecutorImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 * @param sysBean
	 * @param infoBean
	 * @param tableName
	 * @param detailMap
	 * @param inst
	 * @param dataService
	 * @param filePrefix
	 * @param store
	 * @param colName
	 * @param separator
	 * @param ln
	 * @return
	 * @throws Exception
	 */
	private CsvBean notBat(ChangeSysBean sysBean, ChangeInfoBean infoBean,
			String tableName,
			Map<String, List<ChangeTableDetailBean>> detailMap,
			JDAChangeInstBean inst, IJDADataService dataService,
			String filePrefix, String store, String colName, String separator,
			String ln) throws Exception {
		log.info("不需要分批");
		CsvBean bean = null;
		String sql = null;
		String filePath = ConfigUtil.createCSVPath(filePrefix);
		File f = new File(filePath);
		log.info(inst + "----" + filePath);
		bean = new CsvBean(separator, ln, f);
		// 根据每个层 查询出对应的东西
		for (Entry<String, List<ChangeTableDetailBean>> entry : detailMap
				.entrySet()) {
			sql = createDataCountSQL(sysBean.getDbschema(), tableName,
					entry.getKey(), inst, store, colName);
			int count = dataService.searchScanlar(sql);
			log.info("构建的sql:" + sql);
			LayerBean laybean = createLayBean(infoBean, store, f, entry, count);
			int pageSize = 10000;
			// 查询最大的rownum并保持
			String table = sysBean.getDbschema() + "." + tableName;
			String colPamar = "";
			if (colName != null && store != null) {
				colPamar = " and " + colName + "='" + store + "' ";
			}
			// as400数据库不同的地方，就是这个鬼一样的分页，需要记录这个
			int maxRowNum = 0;
			if(sysBean.getDbtype().equals(ChangeSysBean.DB_ORACLE))
			{
				
				maxRowNum = dataService.searchScanlar("SELECT MAX(rownum)+1 FROM " + table + " where INSTNO='"
						+ inst.getInstno() + "' " + colPamar);
			} else if(sysBean.getDbtype().equals(ChangeSysBean.DB_SQLSERVER)){
				
				maxRowNum = dataService.searchScanlar("SELECT count(*) FROM " + table + " where INSTNO='"
						+ inst.getInstno() + "' " + colPamar);
			}
				else
			
			{
					String maxSql = "SELECT MAX(RRN(" + table
							+ "))+1 FROM " + table + " where INSTNO='"
							+ inst.getInstno() + "'" + colPamar;
					log.info("maxRowNum SQL:" + maxSql);
				
					maxRowNum = dataService.searchScanlar(maxSql);
			}
			
			bean.writeLayHead(laybean);
			// 每1000条进行一次查询
			log.info("翻页查找:" + maxRowNum);
			int laySize = 0;
			for (int i = 0; i < count; i += pageSize) {
				// 拼接翻页
				sql = createPageSql(table, entry.getValue(), inst.getInstno(),
						maxRowNum, pageSize, store, colName , sysBean.getDbtype());
				log.info("翻页查找：" + sql);
				//modify 20130809				
//				List<String[]> dataList = dataService.searchData(sql, null,
//						entry.getValue());
				List<String[]> dataList = dataService.searchData(sql, null,
						entry.getValue(),table);

				bean.appendLayData(dataList);
				// 记录最大页数
				maxRowNum -= pageSize;
				log.info("翻页查找:" + maxRowNum);
				laySize += dataList.size();
			}
			// 如果最后一次有余数，肯定需要在查询一次
			if (count > pageSize && count % pageSize > 0) {
				// 拼接翻页
				sql = createPageSql(table, entry.getValue(), inst.getInstno(),
						maxRowNum, pageSize, store, colName , sysBean.getDbtype());
				log.info("翻页查找剩下的：" + sql);
//				List<String[]> dataList = dataService.searchData(sql, null,
//						entry.getValue());
				List<String[]> dataList = dataService.searchData(sql, null,
						entry.getValue(), table);
				bean.appendLayData(dataList);
				log.info("翻页查找剩下的:" + maxRowNum);
				laySize += dataList.size();
			}
			laybean.setSize(laySize);
			bean.wrirteLayFoot(laybean);
			// 添加一个层
			bean.getLayerList().add(laybean);
		}
		bean.writeEnd();
		// 这点要注意哦，路径一定要带出去，后面如果有编程的哥们，这个不带出去会有问题滴
		// bean.setFilePath(filePath);
		// 添加一条记录
		// BatMappingBean bmb = new BatMappingBean();
		// bmb.setChgcode(infoBean.getChgcode());
		// bmb.setInstno(inst.getInstno());
		// bmb.setFilname(bean.getFilePath());
		// this.batMappingService.addMapping(bmb);
		return bean;
	}

	
	private CsvBean notBatForMerge(List<MergeDto> dtoList,String ln) throws Exception {
		
		log.info("不需要分批");
		CsvBean bean = null;
		String sql = null;
		
		
		
		String filePath = ConfigUtil.createCSVPath(dtoList.get(0).getFilePrefix());
		File f = new File(filePath);
		String separator = ConfigUtil.getSeparator(dtoList.get(0).getChangeInfoBean().getSeqcode())
				.toString();
		
		
		log.info(dtoList.get(0).getjDAChangeInstBean() + "----" + filePath);
		bean = new CsvBean(separator, ln, f);
		
		for(MergeDto tMergeDto : dtoList){
			
			
			
			// 根据每个层 查询出对应的东西
			for (Entry<String, List<ChangeTableDetailBean>> entry : tMergeDto.getDetailMap()
					.entrySet()) {
				sql = createDataCountSQL(tMergeDto.getChangeSysBean().getDbschema(), tMergeDto.getChgTbl().getSrctblname(),
						entry.getKey(), tMergeDto.getjDAChangeInstBean(), tMergeDto.getStore(), tMergeDto.getColName());
				int count = tMergeDto.getiJDADataService().searchScanlar(sql);
				log.info("构建的sql:" + sql);
				LayerBean laybean = createLayBean(tMergeDto.getChangeInfoBean(), tMergeDto.getStore(), f, entry, count);
				int pageSize = 10000;
				// 查询最大的rownum并保持
				String table = tMergeDto.getChangeSysBean().getDbschema() + "." + tMergeDto.getChgTbl().getSrctblname();
				String colPamar = "";
				if (tMergeDto.getColName() != null && tMergeDto.getStore() != null) {
					colPamar = " and " + tMergeDto.getColName() + "='" + tMergeDto.getStore() + "' ";
				}
				// as400数据库不同的地方，就是这个鬼一样的分页，需要记录这个
				int maxRowNum = 0;
				if(tMergeDto.getChangeSysBean().getDbtype().equals(ChangeSysBean.DB_ORACLE))
				{
					
					maxRowNum = tMergeDto.getiJDADataService().searchScanlar("SELECT MAX(rownum)+1 FROM " + table + " where INSTNO='"
							+ tMergeDto.getjDAChangeInstBean().getInstno() + "' " + colPamar);
				} else if(tMergeDto.getChangeSysBean().getDbtype().equals(ChangeSysBean.DB_SQLSERVER)){
					
					maxRowNum = tMergeDto.getiJDADataService().searchScanlar("SELECT count(*) FROM " + table + " where INSTNO='"
							+ tMergeDto.getjDAChangeInstBean().getInstno() + "' " + colPamar);
				}else
				{
					String maxSql = "SELECT MAX(RRN(" + table
							+ "))+1 FROM " + table + " where INSTNO='"
							+ tMergeDto.getjDAChangeInstBean().getInstno() + "'" + colPamar;
					log.info("maxRowNum SQL:" + maxSql);
					maxRowNum = tMergeDto.getiJDADataService().searchScanlar(maxSql);
				}
				
				bean.writeLayHead(laybean);
				// 每1000条进行一次查询
				log.info("翻页查找:" + maxRowNum);
				int laySize = 0;
				for (int i = 0; i < count; i += pageSize) {
					// 拼接翻页
					sql = createPageSql(table, entry.getValue(), tMergeDto.getjDAChangeInstBean().getInstno(),
							maxRowNum, pageSize, tMergeDto.getStore(), tMergeDto.getColName() , tMergeDto.getChangeSysBean().getDbtype());
					log.info("翻页查找：" + sql);
					//modify 20130809				
//					List<String[]> dataList = dataService.searchData(sql, null,
//							entry.getValue());
					List<String[]> dataList = tMergeDto.getiJDADataService().searchData(sql, null,
							entry.getValue(),table);

					bean.appendLayData(dataList);
					// 记录最大页数
					maxRowNum -= pageSize;
					log.info("翻页查找:" + maxRowNum);
					laySize += dataList.size();
				}
				// 如果最后一次有余数，肯定需要在查询一次
				if (count > pageSize && count % pageSize > 0) {
					// 拼接翻页
					sql = createPageSql(table, entry.getValue(), tMergeDto.getjDAChangeInstBean().getInstno(),
							maxRowNum, pageSize, tMergeDto.getStore(), tMergeDto.getColName() , tMergeDto.getChangeSysBean().getDbtype());
					log.info("翻页查找剩下的：" + sql);
//					List<String[]> dataList = dataService.searchData(sql, null,
//							entry.getValue());
					List<String[]> dataList = tMergeDto.getiJDADataService().searchData(sql, null,
							entry.getValue(), table);
					bean.appendLayData(dataList);
					log.info("翻页查找剩下的:" + maxRowNum);
					laySize += dataList.size();
				}
				laybean.setSize(laySize);
				bean.wrirteLayFoot(laybean);
				// 添加一个层
				bean.getLayerList().add(laybean);
			}
			
		}
		
		
		bean.writeEnd();
		
		return bean;
		
	}
	
	
	
	private List<CsvBean> isBatForMerge(List<MergeDto> dtoList,String ln) throws Exception {
		log.info("需要分批");
		CsvBean bean = null;
		String sql = null;
		List<CsvBean> list = new ArrayList<CsvBean>();
		String fileName;
		String filePath = ConfigUtil.createCSVPath(dtoList.get(0).getFilePrefix());
		File f = new File(filePath);
		
		log.info(dtoList.get(0).getjDAChangeInstBean() + "----" + filePath);
		fileName = f.getName();
		String separator = ConfigUtil.getSeparator(dtoList.get(0).getChangeInfoBean().getSeqcode())
				.toString();
		bean = new CsvBean(separator, ln, f);
		for(MergeDto tMergeDto : dtoList){
			
			
			Map<String, List<ChangeTableDetailBean>> detailMap = tMergeDto.getDetailMap();
			ChangeSysBean sysBean = tMergeDto.getChangeSysBean();
			String tableName = tMergeDto.getChgTbl().getSrctblname();
			JDAChangeInstBean inst = tMergeDto.getjDAChangeInstBean();
			IJDADataService dataService = tMergeDto.getiJDADataService();
			ChangeInfoBean infoBean = tMergeDto.getChangeInfoBean();
			String filePrefix = tMergeDto.getFilePrefix();
			String store = tMergeDto.getStore();
			String colName = tMergeDto.getColName();
			for (Entry<String, List<ChangeTableDetailBean>> entry : detailMap
					.entrySet()) {


				

				sql = createDataCountSQL(sysBean.getDbschema(), tableName,
						entry.getKey(), inst, store, colName);
				int count = dataService.searchScanlar(sql);
				log.info("构建的sql:" + sql);
				LayerBean laybean = createLayBean(infoBean, store, f, entry, count);
				int pageSize = 10000;
				// 查询最大的rownum并保持
				String table = sysBean.getDbschema() + "." + tableName;
				String colPamar = "";
				if (colName != null && store != null) {
					colPamar = " and " + colName + "='" + store + "' ";
				}
				int maxRowNum = 0;
				// as400数据库不同的地方，就是这个鬼一样的分页，需要记录这个
				if(sysBean.getDbtype().equals(ChangeSysBean.DB_ORACLE))
				{
					
					maxRowNum = dataService.searchScanlar("SELECT MAX(rownum)+1 FROM " + table + " where INSTNO='"
							+ inst.getInstno() + "' " + colPamar);
				} else if(sysBean.getDbtype().equals(ChangeSysBean.DB_SQLSERVER)){
					
					maxRowNum = dataService.searchScanlar("SELECT count(*) FROM " + table + " where INSTNO='"
							+ inst.getInstno() + "' " + colPamar);
				}else
				{
					String maxSql = "SELECT MAX(RRN(" + table
							+ "))+1 FROM " + table + " where INSTNO='"
							+ inst.getInstno() + "' " + colPamar;
					log.info("maxRowNum SQL:" + maxSql);
					maxRowNum = dataService.searchScanlar(maxSql);

				}
				
				bean.writeLayHead(laybean);
				// 每1000条进行一次查询
				log.info("翻页查找:" + maxRowNum);
				int laySize = 0;
				for (int i = 0; i < count; i += pageSize) {
					// 拼接翻页
					sql = createPageSql(table, entry.getValue(), inst.getInstno(),
							maxRowNum, pageSize, store, colName , sysBean.getDbtype());
					log.info("翻页查找：" + sql);
//					List<String[]> dataList = dataService.searchData(sql, null,
//							entry.getValue());
					List<String[]> dataList = dataService.searchData(sql, null,
							entry.getValue(),table);
					bean.appendLayData(dataList);
					// 记录最大页数
					maxRowNum -= pageSize;
					log.info("翻页查找:" + maxRowNum);
					laySize += dataList.size();
					// 开始分文件
					if (laySize >= infoBean.getBatcnt()) {
						laybean.setSize(laySize);
						bean.wrirteLayFoot(laybean);
						// 添加一个层
						bean.getLayerList().add(laybean);
						bean.writeEnd();
						// 将现有的对象添加到集合中
						list.add(bean);
						// 添加一个映射记录
						BatMappingBean bmb = new BatMappingBean();
						bmb.setChgcode(infoBean.getChgcode());
						bmb.setInstno(inst.getInstno());
						bmb.setFilname(fileName);
						this.batMappingService.addMapping(bmb);
						laySize = 0;
						// 重新构建新的对象
						filePath = ConfigUtil.createCSVPath(filePrefix);
						log.info("换文件：" + filePath);
						f = new File(filePath);
						fileName = f.getName();
						bean = new CsvBean(separator, ln, f);
						laybean = createLayBean(infoBean, store, f, entry, count);
						// 层的头部
						bean.writeLayHead(laybean);
					}
				}
				// 如果最后一次有余数，肯定需要在查询一次
				if (count > pageSize && count % pageSize > 0) {
					// 拼接翻页
					sql = createPageSql(table, entry.getValue(), inst.getInstno(),
							maxRowNum, pageSize, store, colName, sysBean.getDbtype());
					log.info("翻页查找剩下的：" + sql);
//					List<String[]> dataList = dataService.searchData(sql, null,
//							entry.getValue());
					List<String[]> dataList = dataService.searchData(sql, null,
							entry.getValue(),table);
					bean.appendLayData(dataList);
					log.info("翻页查找剩下的:" + maxRowNum);
					laySize += dataList.size();
				}
				laybean.setSize(laySize);
				bean.wrirteLayFoot(laybean);
				// 添加一个层
				bean.getLayerList().add(laybean);
				list.add(bean);
				// 添加一个映射记录
				BatMappingBean bmb = new BatMappingBean();
				bmb.setChgcode(infoBean.getChgcode());
				bmb.setInstno(inst.getInstno());
				bmb.setFilname(fileName);
				this.batMappingService.addMapping(bmb);
			}
			
		}
		
		
		return list;
	}
	
	/**
	 * 创建层对象
	 * 
	 * @file: CSVIOExecutorImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-7
	 * @param infoBean
	 * @param store
	 * @param f
	 * @param entry
	 * @param count
	 * @return
	 */
	private LayerBean createLayBean(ChangeInfoBean infoBean, String store,
			File f, Entry<String, List<ChangeTableDetailBean>> entry, int count) {
		LayerBean laybean = new LayerBean();
		laybean.setChgname(f.getName());// 交换资料名称
		laybean.setLayCode(entry.getKey());// 层代码就是map的key
		
		if ("Y".equals(infoBean.getIsallstr())) {
			laybean.setMsgCode(infoBean.getMsgcode());// 电文代码
		} else if ("N".equals(infoBean.getIsallstr())) {
			laybean.setMsgCode(infoBean.getMsgcode() + store);// 电文代码
		}
		laybean.setSysName(infoBean.getSrccode());// 来源系统
		laybean.setSize(count);
		return laybean;
	}

	/**
	 * 根据翻页查询的sql
	 * 
	 * @file: CSVPipe.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-31
	 * @param schema
	 * @param tableName
	 * @param value
	 * @param pageSize
	 * @param maxRowNum
	 * @param instno
	 * @param colName
	 * @param store
	 * @return
	 */
	
	
	
	private String createPageSql(String table,
			List<ChangeTableDetailBean> value, String instno, int maxRowNum,
			int pageSize, String store, String colName , String dbType) 
	{
		String cols = "";
		String sortCols = "";
		// 排序的列，后来加上的代码
		for (ChangeTableDetailBean ctdb : value) {
			cols += "," + ctdb.getSrcfldcode();
			if ("Y".equals(ctdb.getIsseq())) {
				sortCols += "," + ctdb.getSrcfldcode();
			}
		}
		// 这里是很重要的，需要注意，如果是针对单个门店的话，
		// 这个地方就会让数据不一样，所以，需要增加一个条件
		String colPamar = "";
		if (colName != null && store != null) {
			colPamar = " and " + colName + "='" + store + "' ";
		}
		
		
		
		if(ChangeSysBean.DB_ORACLE.equals(dbType))
		{
			String sql = "select " + cols.substring(1) + " from (SELECT rownum as rn " + cols + " from " + table + 
			" WHERE INSTNO='" + instno + "' " + colPamar + 
			" order by rownum desc " + sortCols + ") where rn > " + (maxRowNum - pageSize-1) + " and rn < " + maxRowNum;
		
			return sql;
		} else if(ChangeSysBean.DB_SQLSERVER.equals(dbType)){
			String sql = "SELECT TOP " +pageSize+ " " + cols.substring(1) + " FROM (SELECT id "+cols+"  FROM " +table+ 
			"  WHERE INSTNO='" + instno + "') a WHERE ( id NOT IN ( SELECT TOP " +(maxRowNum / pageSize)+ 
			" id  FROM (SELECT id "+ cols +" FROM "+ table + " WHERE INSTNO='" + instno + "') a ORDER BY id ) ) ORDER BY id";
					
			return sql;
		}else
		{
			// 构建as400 db2的翻页sql，as400的翻页总是这么神奇，害的我捉摸了半天
			return "SELECT RRN(" + table + ") AS a1" + cols + " from " + table
					+ " WHERE INSTNO='" + instno + "' " + colPamar + " AND RRN("
					+ table + ") < " + maxRowNum + " order by a1 desc " + sortCols
					+ " Fetch First " + pageSize + " Rows Only";			
		}
		
	}

	/**
	 * 获得数据的总长度的sql
	 * 
	 * @file: CSVPipe.java
	 * @author 唐植超(上海软通)
	 * @date 2012-12-31
	 * @param schema
	 * @param tableName
	 * @param key
	 * @param inst
	 * @param colName
	 * @param store
	 * @return
	 */
	private String createDataCountSQL(String schema, String tableName,
			String key, JDAChangeInstBean inst, String store, String colName) {
		StringBuilder sql = new StringBuilder();
		sql.append("select count(*) as COUNT from ");
		sql.append(schema);
		sql.append("." + tableName);
		// instno贯穿全局啊,牛逼的东东，哈哈哈
		sql.append(" where instno='" + inst.getInstno() + "'");
		// 追加门店信息的条件，这玩意好，实在啊
		if (colName != null && store != null) {
			sql.append(" and " + colName + "='" + store + "'");
		}
		return sql.toString();
	}

	/**
	 * 输入
	 * 
	 * @file: CSVIOExecutorImpl.java
	 * @author 唐植超(上海软通)
	 * @date 2013-1-6
	 * @param sysBean
	 * @param infoBean
	 * @param srctblname
	 * @param detailMap
	 * @param inst
	 * @param dataService
	 * @param path
	 * @param store
	 * @param colName
	 * @return
	 * @throws Exception
	 */
	private List<CsvBean> in(ChangeSysBean sysBean, ChangeInfoBean infoBean,
			String srctblname,
			Map<String, List<ChangeTableDetailBean>> detailMap,
			JDAChangeInstBean inst, IJDADataService dataService, String path,
			String store, String colName) throws Exception {
		throw new Exception("输入功能没有实现！！！");
	}

	@Override
	public List<CsvBean> execut(ChangeSysBean sysBean, ChangeInfoBean infoBean,
			JDAChangeInstBean inst, IJDADataService dataService,
			ChangeTableBean chgTbl,
			Map<String, List<ChangeTableDetailBean>> detailMap,
			String filePrefix, String store, String colName) throws Exception {
			System.out.println(inst.getInstno()+"=========SOM  批次进入 状态:"+infoBean.getIotype());
		if ("O".equals(infoBean.getIotype())) {
			return out(sysBean, infoBean, chgTbl.getSrctblname(), detailMap,
					inst, dataService, filePrefix, store, colName);
		}
		if ("I".equals(infoBean.getIotype())) {
		
			return in(sysBean, infoBean, chgTbl.getSrctblname(), detailMap,
					inst, dataService, filePrefix, store, colName);
		}
		throw new Exception("配置Iotype不正确，请检查！！");
	}

	@Override
	public List<CsvBean> executForMerge(List<MergeDto> dtoList)
			throws Exception {
		// TODO Auto-generated method stub
	
			System.out.println(dtoList.get(0).getjDAChangeInstBean().getInstno()+"=========SOM  批次进入 状态:"+dtoList.get(0).getChangeInfoBean().getIotype());
			if ("O".equals(dtoList.get(0).getChangeInfoBean().getIotype())) {
				return outForMerge(dtoList);
			}
			if ("I".equals(dtoList.get(0).getChangeInfoBean().getIotype())) {
				
				throw new Exception("输入功能没有实现！！！");
			}
			throw new Exception("配置Iotype不正确，请检查！！");
		
	}

}
