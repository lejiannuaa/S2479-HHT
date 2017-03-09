package com.hola.tarjda.service;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Map;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.hola.common.constant.DefvalForChgtbldtl;
import com.hola.common.csv.model.DataBean;
import com.hola.common.csv.model.LayerBean;
import com.hola.common.dao.HhtserverDao;
import com.hola.common.dao.StoreDao;
import com.hola.common.exception.BscfgException;
import com.hola.common.exception.SendMqException;
import com.hola.common.file.FileManager;
import com.hola.common.model.Chginst;
import com.hola.common.model.mw.BsLog;
import com.hola.common.model.mw.ChgTbl;
import com.hola.common.model.mw.ChgTblDtl;
import com.hola.common.model.mw.Chginfo;
import com.hola.common.model.mw.StoreInfo;
import com.hola.common.mq.JmsHelper;
import com.hola.common.service.BaseService;
import com.hola.common.util.CommonUtil;
import com.hola.common.util.StringUtil;
import com.hola.common.util.ThreadPoolUtil;
import com.hola.common.util.TimeUtil;
import com.hola.common.util.Uuid16;
import com.hola.common.util.ZipUtils;
import com.hola.tarjda.GetDataForBsSendMq;
import com.hola.tarjda.cache.ConfigCache;


public class TarJdaService extends BaseService
{
	private static Log log = LogFactory.getLog(TarJdaService.class);
	
	private		StoreDao		storeDao;
	private		HhtserverDao	hhtserverDao;
	
	
	
	@SuppressWarnings("static-access")
	public void createCsvString(String storeNo)
	{
		StoreInfo storeInfo = hhtserverDao.getStoreInfoForStoreNo(storeNo);
		
		log.info("抛转门店" + storeNo + "的待抛转数据开始..........");
		String schema = storeInfo.getDbSchema();
		//获取状态为未发送的接口头档，并更新接口头档状态为发送中chginst.chgsts='0',并且chgcode数据在此交换系统负责的范围内
		String chgcodeStr = ConfigCache.getInstance().getAllChgcodeStrForIOType(HhtserverDao.IOTYPE_OUT);
		List<Chginst> chginstList = storeDao.getChginstNoUpload(schema , chgcodeStr);
		
		log.info("门店" + storeNo + "待抛转的有" + chginstList.size() + "个批次!开始循环抛转.....");
		//构建MQ帮助对象
		//MqHelper mqHelper = new MqHelper(storeInfo.getMqIp(), new Integer(storeInfo.getQMGPort()), storeInfo.getMqUserName(), storeInfo.getMqPwd(), storeInfo.getQMGName(), storeInfo.getOQName());
		JmsHelper jmsHelper = new JmsHelper(storeInfo.getMqIp(), new Integer(storeInfo.getQMGPort()), storeInfo.getMqUserName(), storeInfo.getMqPwd(), storeInfo.getQMGName(), storeInfo.getOQName());
		GetDataForBsSendMq.instnoSum += chginstList.size();
		for (Chginst chginst : chginstList) 
		{
			
			ExecutionRun run = new ExecutionRun(storeInfo, chginst, jmsHelper);
			//防止线程池队列满，丢失当前chginst
			
			
			while(true)
			{
				try
				{
					log.info("开始抛转instno:" + chginst.getInstno());
					ThreadPoolUtil.runJob(run);
					break;
				} catch(java.util.concurrent.RejectedExecutionException e)
				{
					log.info("线程池队列已满，当前instno：" + chginst.getInstno());
					try {
						Thread.currentThread().sleep(1000);
					} catch (InterruptedException e1) {
						e1.printStackTrace();
					}
				}
			}
		}
	}
	
	class ExecutionRun implements Runnable
	{
		StoreInfo storeInfo;
		Chginst chginst;
		JmsHelper mqHelper;
		
		private ExecutionRun(StoreInfo storeInfo, Chginst chginst,
				JmsHelper mqHelper) 
		{
			this.storeInfo = storeInfo;
			this.chginst = chginst;
			this.mqHelper = mqHelper;
		}
		
		@Override
		public void run() 
		{
			Thread.currentThread().setName(Uuid16.create().toString());
			
			BsLog bsLog = new BsLog();
			bsLog.setCrtTime(TimeUtil.formatDate(new Date(), TimeUtil.DATE_FORMAT_yyyyMMddHHmmss));
			bsLog.setId(Thread.currentThread().getName());
			bsLog.setInstno(chginst.getInstno());
			bsLog.setStatus(BsLog.STATUS_SUCESS);
			bsLog.setStoreNo(storeInfo.getStoreNo());
			
			log.info("开始获取并抛转" + storeInfo.getStoreNo() + "门店,CHGCODE:" + chginst.getChgcod() + "INSTNO:" + chginst.getInstno() +"数据.....");
			try {
				dataToCsvUploadMq(storeInfo , chginst , mqHelper , bsLog);
				//更新接口头档状态为已发送
				storeDao.updateChginstSts(storeInfo.getDbSchema(), StoreDao.CHGINST_STS_UPLOAD_SUCESS, chginst.getInstno());
			} catch (Exception e) 
			{
				//恢复接口头档成未发送
				storeDao.updateChginstSts(storeInfo.getDbSchema(), StoreDao.CHGINST_STS_UPLOAD_NOT, chginst.getInstno());
				e.printStackTrace();
				bsLog.setRemark(e.getMessage());
				bsLog.setStatus(BsLog.STATUS_FAILURE);
				log.error("获取数据转换csv并发送mq过程中，出现异常!!!instno:" + chginst.getInstno() , e);
				
			}
			log.info("执行" + storeInfo.getStoreNo() + "的" + chginst.getInstno() + "完成!!!");
			hhtserverDao.addBsLog(bsLog);
		}
	}
	
	private	void dataToCsvUploadMq(StoreInfo storeInfo , Chginst chginst , JmsHelper mqHelper, BsLog bsLog) throws BscfgException, SendMqException
	{
		Chginfo chginfo = null;
		//需要提升性能，可以在此加缓存处理!!!
		chginfo = ConfigCache.getInstance().getChginfoForChgcode(chginst.getChgcod());//使用缓存
		log.info("读取配置成功!,chgcode:" + chginfo.getChgcode());
		List<LayerBean> csvLayers = initLayBean(chginfo , storeInfo.getStoreNo());
		log.info("初始化csv对象成功!!!");
		loadDataAndFillLayer(csvLayers , chginfo.getChgtblList() , storeInfo.getDbSchema() , chginst.getInstno());
		//校验抛转笔数是否大于0，如果为0不抛转MQ
		for (int i = 0; i < csvLayers.size(); i++) 
		{
			LayerBean csvLayer = csvLayers.get(i);
			if(csvLayer.getDataList().size() > 0)
				break;
			if(i == csvLayers.size() - 3)
			{
				log.info("instno:" + chginst.getInstno() + "的抛转笔数为0，返回此批次，不推送MQ任何信息!!!");
				return;
			}
		}
		
		String csvString = getCsvStr(chginfo , csvLayers);
		File zipFile = null;
		try {
			File directory = FileManager.getCsvFileDirectoryForBsToMq(storeInfo.getStoreNo());
			File file = new File(directory, csvLayers.get(0).getChgname());
			file.createNewFile();
			FileManager.writeStringToFile(file, csvString);
			log.info("写文件成功!" + file.getPath());
			zipFile = new File(directory , csvLayers.get(0).getChgnamePrefix() + ".zip");
			ZipUtils z = new ZipUtils();
			z.zipFile(file, zipFile);
			log.info("压缩文件成功!" + zipFile.getPath());
			log.info("发送MQ开始!!!:" + mqHelper.getMqDetail());
			mqHelper.sendFile(zipFile);
			log.info("已成功发送MQ" + zipFile.getPath());
			bsLog.setFilName(StringUtil.replaceAll(zipFile.getPath(), "\\", "/"));
		} catch (IOException e) 
		{
			log.error("获取文件路径或写入文件出错" , e);
			e.printStackTrace();
		}
		log.info("发送MQ成功!!!:");
		//通过读取数据库配置，修改view状态为已上传
		updateView(chginfo , storeInfo.getDbSchema() , chginst.getInstno());
		
		/* 作废，此调用方法为读取配置更新view this.storeDao.updateView(storeInfo.getDbSchema(), chginst.getInstno(), chginfo.getChgcode()); */
		
		
		log.info("修改view成功!!!");
	}
	@SuppressWarnings("unused")
	private File dataToCsv(StoreInfo storeInfo, Chginst chginst)  throws BscfgException
	{
		Chginfo chginfo = null;
		//需要提升性能，可以在此加缓存处理!!!
		chginfo = ConfigCache.getInstance().getChginfoForChgcode(chginst.getChgcod());//使用缓存
		log.info("读取配置成功!,chgcode:" + chginfo.getChgcode());
		List<LayerBean> csvLayers = initLayBean(chginfo , storeInfo.getStoreNo());
		loadDataAndFillLayer(csvLayers , chginfo.getChgtblList() , storeInfo.getDbSchema() , chginst.getInstno());
		String csvString = getCsvStr(chginfo , csvLayers);
		try {
			File directory = FileManager.getCsvFileDirectoryForBsToMq(storeInfo.getStoreNo());
			File file = new File(directory, csvLayers.get(0).getChgname());
			file.createNewFile();
			FileManager.writeStringToFile(file, csvString);
			log.info("写文件成功!" + file.getPath());
			File zipFile = new File(directory , csvLayers.get(0).getChgnamePrefix() + ".zip");
			ZipUtils z = new ZipUtils();
			z.zipFile(file, zipFile);
			log.info("压缩文件成功!" + zipFile.getPath());
			
			//通过读取数据库配置，修改view状态为已上传
			updateView(chginfo , storeInfo.getDbSchema() , chginst.getInstno());
			
			/* 作废，此调用方法为读取配置更新view this.storeDao.updateView(storeInfo.getDbSchema(), chginst.getInstno(), chginfo.getChgcode()); */
			log.info("修改view成功!!!");
			return zipFile;
			
		} catch (IOException e) 
		{
			log.error("获取文件路径或写入文件出错" , e);
			e.printStackTrace();
		}
		return null;
	}
	
	
	private void updateView(Chginfo chginfo, String dbSchema, String instno) 
	{
		List<ChgTbl> chgtblList = chginfo.getChgtblList();
		for (ChgTbl chgTbl : chgtblList) 
		{
			if(!"null".equals(chgTbl.getSrcOpcond()) && StringUtil.isEmpty(chgTbl.getSrcOpcond()) == false)
			{
				this.storeDao.updateViewForDb(dbSchema, instno , chgTbl.getSrcOpcond());
			}
		}
	}
	private String getCsvStr(Chginfo chginfo, List<LayerBean> csvLayers) 
	{
		DataBean dataBean = new DataBean();
		dataBean.setLayerList(csvLayers);
		if("TAB".equals(chginfo.getSeqcode().trim()))
			return dataBean.toCsv("\t", "\n");
		else
			return dataBean.toCsv(chginfo.getSeqcode(), "\n");
	}

	@SuppressWarnings("rawtypes")
	private void loadDataAndFillLayer(List<LayerBean> csvLayers, List<ChgTbl> chgtblList , String schema , String instno)
	{
		for (ChgTbl chgTbl : chgtblList) 
		{
			List<String []> datas = new ArrayList<String[]>();
			List queryList = this.storeDao.getData(chgTbl.getColStr() , chgTbl.getSrcTableName() , schema , instno);
			
			List<ChgTblDtl> chgTblDtlList = chgTbl.getCols();
			
			//循环获取所有行
			for (Object obj : queryList) 
			{
				String [] data = new String [chgTblDtlList.size()];
				//循环获取所有列
				Map map = (Map) obj;
				for (int i = 0; i < chgTblDtlList.size(); i++) 
				{
					ChgTblDtl currentChgTblDtl = chgTblDtlList.get(i);
					//如果有默认值，优先赋值默认值
					if(currentChgTblDtl.getDefaultValue() != null && currentChgTblDtl.getDefaultValue().equals("") == false)
					{
						//如果DEFAULT值是关键字INSTNO，赋值当前的instno
						if(currentChgTblDtl.getDefaultValue().equals(DefvalForChgtbldtl.INSTNO))
							data[i] = instno;
						else
							data[i] = currentChgTblDtl.getDefaultValue();
					} else
					{
						Object valueObj = map.get(currentChgTblDtl.getSrcFldCode());
						if(valueObj == null || valueObj.equals(""))
							valueObj = " ";
						data[i] = valueObj + "";
					}
				}
				datas.add(data);
			}
			
			//将数据存入对应的Layer中
			LayerBean layer = csvLayers.get(chgTbl.getLayNo() - 1);
			layer.setDataList(datas);
		}
	}

	private List<LayerBean> initLayBean(Chginfo chginfo , String storeNo) 
	{
		List<ChgTbl> chgtblList = chginfo.getChgtblList();
		List<LayerBean> layerBeanList = new ArrayList<LayerBean>();
		LayerBean [] layerBeans = new LayerBean [chgtblList.size()];
		
		String chgname = CommonUtil.getInstance().getChgName(chginfo.getMsgcode() , storeNo);
		for (ChgTbl chgtbl : chgtblList) 
		{
			LayerBean layer = new LayerBean();
			layer.setChgname(chgname);
			layer.setSysName(chginfo.getSrccode());
			layer.setLayCode("H" + chgtbl.getLayNo());
			layer.setMsgCode(CommonUtil.getMsgcode(chginfo.getMsgcode() , storeNo));
			layerBeans[chgtbl.getLayNo() - 1] = layer;
		}
		
		for (int i = 0; i < layerBeans.length; i++) 
		{
			layerBeanList.add(layerBeans[i]);
		}
		
		return layerBeanList;
	}

	public Chginfo loadSrcConfig(String chgcode) throws BscfgException 
	{
		Chginfo chginfo = hhtserverDao.getChginfoForChgcode(chgcode);
		List<ChgTbl> chgtblList = hhtserverDao.getTblForChgcode(chgcode);
		for (ChgTbl chgtbl : chgtblList) 
		{
			hhtserverDao.getTblDtlForSrc(chgtbl);
		}
		chginfo.setChgtblList(chgtblList);
		return chginfo;
	}
	
	public List<String> getStoreNoList()
	{
		List<String> storeNoList = hhtserverDao.getAllStoreNo();
		return storeNoList;
	}

	public StoreDao getStoreDao() {
		return storeDao;
	}
	public void setStoreDao(StoreDao storeDao) {
		this.storeDao = storeDao;
	}
	public HhtserverDao getHhtserverDao() {
		return hhtserverDao;
	}
	public void setHhtserverDao(HhtserverDao hhtserverDao) {
		this.hhtserverDao = hhtserverDao;
	}
}
