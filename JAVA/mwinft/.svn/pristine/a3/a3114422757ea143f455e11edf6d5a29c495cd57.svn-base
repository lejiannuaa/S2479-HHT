package com.hola.jda2hht.core.pipe.impl;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.Logger;

import com.hola.jda2hht.core.med.ITerminal;
import com.hola.jda2hht.model.ChangeInfoBean;
import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.model.ChangeTableBean;
import com.hola.jda2hht.model.ChangeTableDetailBean;
import com.hola.jda2hht.model.CsvBean;
import com.hola.jda2hht.model.JDAChangeInstBean;
import com.hola.jda2hht.model.JDAChangeInstfBean;
import com.hola.jda2hht.model.MergeDto;
import com.hola.jda2hht.service.IJDAChgInstfService;
import com.hola.jda2hht.service.IJDADataService;
import com.hola.jda2hht.service.impl.JDAChgInstfServiceImpl;
import com.hola.jda2hht.service.impl.JDADataService;
import com.hola.jda2hht.util.ConfigUtil;

public class CSVPipe extends BasePipe {

	private static final Logger log = Logger.getLogger(CSVPipe.class);

	public void tran(ChangeSysBean sysBean, ChangeInfoBean infoBean,
			JDAChangeInstBean inst) throws Exception {
		log.debug("进入csv的数据传输通道");
		System.out.println("===进入csv的数据传输通道 start===");
		List<CsvBean> list;
		ChangeTableBean chgTbl = null;
		Map<String, List<ChangeTableDetailBean>> detailMap = null;
		List<MergeDto> dtoList = new ArrayList<MergeDto>();
		Map<String, List<ChangeTableDetailBean>> detailMapMerge = null;
		// 这个业务对象必须每次都创建，因为他是动态的哦
		IJDAChgInstfService chgInstfService = new JDAChgInstfServiceImpl(
				sysBean);
		// 这个也是，我之所以在这里创建是想让他少实例化几次，不然每个循环都实例化
		IJDADataService dataService = new JDADataService(sysBean);
		// 找到需要交换的表
		List<JDAChangeInstfBean> itfList = chgInstfService.getAllIntList(inst
				.getInstno());
		// 获得传输的终端
		ITerminal itm = ConfigUtil.getTerminal(infoBean.getChgmed());
		// 保存上传的数据信息
		List<CsvBean> dbList = new ArrayList<CsvBean>();
		
		if("EC_A2F".equals(infoBean.getChgcode().toUpperCase())||"EC_A1F".equals(infoBean.getChgcode().toUpperCase())||
				"HHT_INVSCD".equals(infoBean.getChgcode().toUpperCase())||"JDA_NLD".equals(infoBean.getChgcode().toUpperCase())||
				"CRM_A4FCRM".equals(infoBean.getChgcode().toUpperCase())||"CRM_A3FCRM".equals(infoBean.getChgcode().toUpperCase())||
				"CRM_C2FCRM".equals(infoBean.getChgcode().toUpperCase())||"CRM_JDAN".equals(infoBean.getChgcode().toUpperCase())||
				"BOS_A3F".equals(infoBean.getChgcode().toUpperCase())||"BOS_A4F".equals(infoBean.getChgcode().toUpperCase())||
				"BOS_A5F".equals(infoBean.getChgcode().toUpperCase())||"BOS_C1F".equals(infoBean.getChgcode().toUpperCase())||
				"BOS_C6F".equals(infoBean.getChgcode().toUpperCase())||"BOS_D1F".equals(infoBean.getChgcode().toUpperCase())||
				"BOS_E1F".equals(infoBean.getChgcode().toUpperCase())||"BOS_H1F".equals(infoBean.getChgcode().toUpperCase())||
				"BOS_H2F".equals(infoBean.getChgcode().toUpperCase())||"EC_C1F".equals(infoBean.getChgcode().toUpperCase()))
		{
			JDAChangeInstfBean chginstInstf = new JDAChangeInstfBean();
			chginstInstf.setSrcnam("CHGINST");
			itfList.add(chginstInstf);
			JDAChangeInstfBean chginstfInstf = new JDAChangeInstfBean();
			chginstfInstf.setSrcnam("CHGINSTF");
			itfList.add(chginstfInstf);
		}
		

			
		
		
		for (JDAChangeInstfBean itf : itfList) {
			// 根据表名和交换编号查询出 需要交换的表的id 注意这里 是一个表，一个交换代码 ，所有出来的是一条信息
			
			chgTbl = chgTblService.getChageTable(infoBean.getChgcode(),itf.getSrcnam());
			if (chgTbl == null) {
				throw new Exception("没有查到信息，交换代码：" + infoBean.getChgcode()
						+ " 原表名：" + itf.getSrcnam() + " 配置得应该不正确");
			}
			detailMap = chgTblDtlService.getAllTableDetail(chgTbl.getId());
			
			if("Y".equals(infoBean.getIsmerge())){
				
				if ("Y".equals(infoBean.getIsallstr())) {
					// 先查询所有的门店号，依次调用
					log.info("上传所有的门店，状态位：" + infoBean.getIsallstr());
					
					MergeDto dto = new MergeDto();
					dto.setChangeSysBean(sysBean);
					dto.setChangeInfoBean(infoBean);
					dto.setChgTbl(chgTbl);
					dto.setDetailMap(detailMap);
					dto.setiJDADataService(dataService);
					dto.setjDAChangeInstBean(inst);
					dto.setFilePrefix(infoBean.getMsgcode());
					dto.setStore(null);
					dto.setColName(null);
					dtoList.add(dto);
					
				}  else if ("N".equals(infoBean.getIsallstr())) {
					
					
					log.info("根据门店号上传，状态位：" + infoBean.getIsallstr());
					// 根据TableId 和状态是否为Y，到明细表里面 找出要查询的字段
					ChangeTableDetailBean ctdb = this.chgTblDtlService
							.getDetailBean(chgTbl.getId(), "Y");
					if (ctdb == null) {
						throw new Exception("数据库配置表有问题，" + chgTbl.getSrctblname()
								+ "没有配置门店字段!!");
					}
					
					
					List<String> storeStrList = dataService.searchStoreByInstNo(
							inst.getInstno(), sysBean.getDbschema(),
							chgTbl.getSrctblname(), ctdb.getSrcfldcode());
					// 判断门店号是否为启用状态
					storeStrList = this.storeInfoService.checkEnable(storeStrList);
					
					for (String store : storeStrList) {
						// 直接就是 电文代码+门店号+_+时间戳（精确到3位毫秒数）
						String filePrefix = infoBean.getMsgcode() + store;
						// 构建第一份文件很重要，因为后面的文件都是从这里复制出来的
						MergeDto dto = new MergeDto();
						dto.setChangeSysBean(sysBean);
						dto.setChangeInfoBean(infoBean);
						dto.setChgTbl(chgTbl);
						dto.setDetailMap(detailMap);
						dto.setiJDADataService(dataService);
						dto.setjDAChangeInstBean(inst);
						dto.setFilePrefix(filePrefix);
						dto.setStore(store);
						dto.setColName(ctdb.getSrcfldcode());
						dtoList.add(dto);
					}
				}
				
				
			}else{
				
				// 先判断info表中的标志字段是不是N,如果是N就是发送到固定的几家店，需要去查询
				// 如果是Y，则是发送到所有的点
				if ("Y".equals(infoBean.getIsallstr())) {
					// 先查询所有的门店号，依次调用
					log.info("上传所有的门店，状态位：" + infoBean.getIsallstr());
					// 只写一个文件 直接就是 电文代码+_+时间戳（精确到3位毫秒数）
					list = executor.execut(sysBean, infoBean, inst, dataService,
							chgTbl, detailMap, infoBean.getMsgcode(), null, null);
					if (list == null) {
						throw new Exception("创建csv对象失败");
					}
					dbList.addAll(list);
				} else if ("N".equals(infoBean.getIsallstr())) {
					// 获得要上传的门店号，依次调用
					log.info("根据门店号上传，状态位：" + infoBean.getIsallstr());
					// 根据TableId 和状态是否为Y，到明细表里面 找出要查询的字段
					ChangeTableDetailBean ctdb = this.chgTblDtlService
							.getDetailBean(chgTbl.getId(), "Y");
					if (ctdb == null) {
						throw new Exception("数据库配置表有问题，" + chgTbl.getSrctblname()
								+ "没有配置门店字段!!");
					}
					// 查询出所有的门店号
					List<String> storeStrList = dataService.searchStoreByInstNo(
							inst.getInstno(), sysBean.getDbschema(),
							chgTbl.getSrctblname(), ctdb.getSrcfldcode());
					// 判断门店号是否为启用状态
					storeStrList = this.storeInfoService.checkEnable(storeStrList);
					// 根据门店
					for (String store : storeStrList) {
						// 直接就是 电文代码+门店号+_+时间戳（精确到3位毫秒数）
						String filePrefix = infoBean.getMsgcode() + store;
						// 构建第一份文件很重要，因为后面的文件都是从这里复制出来的
						list = executor.execut(sysBean, infoBean, inst,
								dataService, chgTbl, detailMap, filePrefix, store,
								ctdb.getSrcfldcode());
						if (list == null) {
							throw new Exception("创建csv对象失败");
						}
						dbList.addAll(list);
					}
				}
			}
			// 查询出表的列，层代码是key，每个层的列的集合是value值
			
		}
		if(dtoList.size()>0&&dtoList!=null){
			dbList.addAll(executor.executForMerge(dtoList));
		}
		
		
		itm.upload(infoBean, dbList);
		System.out.println("===进入csv的数据传输通道 end===");
		log.debug("csv通道传输结束");
	}
	
}
