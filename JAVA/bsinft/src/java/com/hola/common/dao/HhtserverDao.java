package com.hola.common.dao;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import com.hola.common.exception.BscfgException;
import com.hola.common.model.mw.BsLog;
import com.hola.common.model.mw.ChgTbl;
import com.hola.common.model.mw.ChgTblDtl;
import com.hola.common.model.mw.Chginfo;
import com.hola.common.model.mw.StoreInfo;
import com.hola.common.util.CommonUtil;
import com.hola.common.util.StringUtil;

public class HhtserverDao extends BaseDao 
{
	public static final String IOTYPE_IN 	= "I";
	public static final String IOTYPE_OUT	= "O";
	
	private static Log log = LogFactory.getLog(HhtserverDao.class);
	/**
	 * 获取门店对应的详细信息，信息内容包括：门店对应的MQ详细信息
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2012-12-27 下午3:59:32
	 * @param storeNo
	 * @return
	 */
	@SuppressWarnings("rawtypes")
	public StoreInfo getStoreInfoForStoreNo(String storeNo)
	{
		String sql = "select STORENO,MQIP,MQUSERNAME,MQPWD,QMGNAME,QMGPORT,OQNAME,IQNAME,AQNAME,QCCSID,DBSCHE " +
				"from hola_app_bscfg_storeinfo where STORENO='" + storeNo + "'";
		List list = super.getHhtserverJt().queryForList(sql);
		for (Object object : list) 
		{
			Map map = (Map) object;
			return fillStoreInfo(map);
		}
		return null;
	}
	
	/**
	 * 获取hhtserver中的所有门店
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2012-12-27 下午3:59:32
	 * @return
	 */
	@SuppressWarnings("rawtypes")
	public List<StoreInfo> getAllStoreInfo()
	{
		List<StoreInfo> totle = new ArrayList<StoreInfo>();
		String sql = "select STORENO,MQIP,MQUSERNAME,MQPWD,QMGNAME,QMGPORT,OQNAME,IQNAME,AQNAME,QCCSID,DBSCHE " +
				"from hola_app_bscfg_storeinfo where STATUS='E'";
		List list = super.getHhtserverJt().queryForList(sql);
		for (Object object : list) 
		{
			Map map = (Map) object;
			StoreInfo si = fillStoreInfo(map);
			totle.add(si);
		}
		return totle;
	}
	/**
	 * 通过chgcode获取表映射信息
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2012-12-27 下午4:11:14
	 * @param chgcode
	 * @return
	 */
	public List<ChgTbl> getTblForChgcode(String chgcode)
	{
		String sql = "select ID,CHGCODE,SRCTBLNAME,TARTBLNAME,TAROPCOND,SRCOPCOND from hola_app_bscfg_chgtbl" +
				" where CHGCODE='" + chgcode + "'";
		return getTbl(sql);
	}
	@SuppressWarnings("rawtypes")
	private List<ChgTbl> getTbl(String sql) 
	{
		List<ChgTbl> totle = new ArrayList<ChgTbl>();
		List list = super.getHhtserverJt().queryForList(sql);
		for (Object object : list) 
		{
			Map map = (Map) object;
			ChgTbl chgtbl = fillChgTbl(map);
			totle.add(chgtbl);
		}
		return totle;
	}
	
	/**
	 * 获取配置档中源字段信息，会将字段信息封装在传入此方法的tbl中。主要信息包括：
	 * 层号，比如H1会存成1；字段集合的字符串，比如col1,col2....
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2012-12-28 下午1:50:11
	 * @param chgtbl
	 * @throws BscfgException
	 */
	@SuppressWarnings("rawtypes")
	public void getTblDtlForSrc(ChgTbl chgtbl) throws BscfgException
	{
		String sql = "select ID,TBLID,SRCLAYCODE,SRCFLDCODE,SRCSEQ,TARLAYCODE,TARFLDCODE,TARSEQ,ISPK,DEFVAL from hola_app_bscfg_chgtbldtl" +
				" where TBLID='" + chgtbl.getId() + "' order by SRCSEQ";
		chgtbl.setCols(new ArrayList<ChgTblDtl>());
		List list = super.getHhtserverJt().queryForList(sql);
		boolean isFirst = true;
		StringBuffer sqlForCol = new StringBuffer();
		for (Object object : list) 
		{
			Map map = (Map) object;
			ChgTblDtl tblDtl = fillChgTblDtl(map);
			if(StringUtil.isEmpty(tblDtl.getSrcFldCode()))
			{
				BscfgException e = new BscfgException("调用dao层getTblDtlForSrc方法时候出错!源字段为空,tblId:" + 
														chgtbl.getId());
				log.error("调用dao层getTblDtlForSrc方法时候出错!源字段为空,tblId:" + chgtbl.getId(), e);
				throw e;
			}
			chgtbl.getCols().add(tblDtl);
			
			//拼装字段信息
			if(isFirst == false)
				sqlForCol.append(",");
			else
			{
				Integer layNo = CommonUtil.laycodeToLayno(tblDtl.getSrcLayCode());
				chgtbl.setLayNo(layNo.intValue());
			}
			sqlForCol.append(tblDtl.getSrcFldCode());
			isFirst = false;
			//设置instno字段所在的列序号
			if(tblDtl.getSrcFldCode().equals("INSTNO"))
				chgtbl.setInstnoColNo(tblDtl.getSrcSeq());
		}
		chgtbl.setColStr(sqlForCol.toString());
	}
	@SuppressWarnings("rawtypes")
	public List<String> getAllChgcodeByIOType(String ioType)
	{
		List<String> chgcodes = new ArrayList<String>();
		String sql = "select CHGCODE from hola_app_bscfg_chginfo where STATUS='E' and IOTYPE='" + ioType + "'";
		List list = super.getHhtserverJt().queryForList(sql);
		for (Object object : list) 
		{
			Map map = (Map) object;
			chgcodes.add(map.get("CHGCODE") + "");
		}
		return chgcodes;
	}
	@SuppressWarnings("rawtypes")
	public void getTblDtlForTar(ChgTbl chgtbl) throws BscfgException
	{
		String sql = "select ID,TBLID,SRCLAYCODE,SRCFLDCODE,SRCSEQ,TARLAYCODE,TARFLDCODE,TARSEQ,ISPK,DEFVAL from hola_app_bscfg_chgtbldtl" +
				" where TBLID='" + chgtbl.getId() + "' order by TARSEQ";
		chgtbl.setCols(new ArrayList<ChgTblDtl>());
		List list = super.getHhtserverJt().queryForList(sql);
		boolean isFirst = true;
		
		StringBuffer sqlForCol = new StringBuffer();
		StringBuffer pkForCol = new StringBuffer();
		
		for (Object object : list) 
		{
			Map map = (Map) object;
			ChgTblDtl tblDtl = fillChgTblDtl(map);
			if(StringUtil.isEmpty(tblDtl.getTarFldCode()))
			{
				BscfgException e = new BscfgException("调用dao层getTblDtlForTar方法时候出错!源字段为空,tblId:" + 
														chgtbl.getId());
				log.error("调用dao层getTblDtlForTar方法时候出错!目标字段为空,tblId:" + chgtbl.getId(), e);
				throw e;
			}
			chgtbl.getCols().add(tblDtl);
			
			//拼装字段信息
			if(isFirst == false)
			{
				sqlForCol.append(",");
			} else 
			{
				Integer layNo = CommonUtil.laycodeToLayno(tblDtl.getTarLayCode());
				chgtbl.setLayNo(layNo.intValue());
			}
			sqlForCol.append(tblDtl.getTarFldCode());
			//设置instno字段所在的列序号
			if(tblDtl.getTarFldCode().equals("INSTNO"))
				chgtbl.setInstnoColNo(tblDtl.getTarSeq());
			
			//拼装主键
			if("Y".equals(tblDtl.getIsPk()))
			{
				if(chgtbl.getPkColNo().size() > 0)
					pkForCol.append(",");
				pkForCol.append(tblDtl.getTarFldCode());
				chgtbl.getPkColNo().add(tblDtl.getTarSeq());
			}
				
			isFirst = false;
		}
		chgtbl.setColStr(sqlForCol.toString());
		chgtbl.setPkColStr(pkForCol.toString());
	}
	
	
	public Chginfo getChginfoForChgcode(String chgcode)
	{
		String sql = "select CHGCODE,IOTYPE,SRCCODE,MSGCODE,TAROPTYP,SEQCODE from hola_app_bscfg_chginfo where " +
					" CHGCODE='" + chgcode + "'";
		return getChginfo(sql);
	}
	public Chginfo getChginfoForMsgcode(String msgcode)
	{
		String sql = "select CHGCODE,IOTYPE,SRCCODE,MSGCODE,TAROPTYP,SEQCODE from hola_app_bscfg_chginfo where " +
					" MSGCODE='" + msgcode + "'";
		return getChginfo(sql);
	} 
	@SuppressWarnings("rawtypes")
	private Chginfo getChginfo(String sql) {
		List list = super.getHhtserverJt().queryForList(sql);
		for (Object object : list) {
			Map map = (Map) object;
			return fillChginfo(map);
		}
		return null;
	}

	@SuppressWarnings("rawtypes")
	private Chginfo fillChginfo(Map map) 
	{
		Chginfo chginfo = new Chginfo();
		chginfo.setChgcode((String)map.get("CHGCODE"));
		chginfo.setIotype((String)map.get("IOTYPE"));
		chginfo.setSrccode((String)map.get("SRCCODE"));
		chginfo.setMsgcode((String)map.get("MSGCODE"));
		chginfo.setTaroptyp(map.get("TAROPTYP") + "");
		chginfo.setSeqcode(map.get("SEQCODE") + "");
		return chginfo;
	}
	@SuppressWarnings("rawtypes")
	private ChgTblDtl fillChgTblDtl(Map map) 
	{
		ChgTblDtl ctd = new ChgTblDtl();
		ctd.setId((String)map.get("ID"));
		ctd.setTblId((String)map.get("TBLID"));
		ctd.setSrcLayCode((String)map.get("SRCLAYCODE"));
		ctd.setSrcFldCode((String)map.get("SRCFLDCODE"));
		ctd.setSrcSeq(CommonUtil.getIntegerForObj(map.get("SRCSEQ")));
		ctd.setTarLayCode((String)map.get("TARLAYCODE"));
		ctd.setTarFldCode((String)map.get("TARFLDCODE"));
		ctd.setTarSeq(CommonUtil.getIntegerForObj(map.get("TARSEQ")));
		ctd.setIsPk((String)map.get("ISPK"));
		ctd.setDefaultValue((String)map.get("DEFVAL"));
		return ctd;
	}
	@SuppressWarnings("rawtypes")
	private ChgTbl fillChgTbl(Map map) 
	{
		ChgTbl chgtbl = new ChgTbl();
		chgtbl.setId((String)map.get("ID"));
		chgtbl.setChgcode((String)map.get("CHGCODE"));
		chgtbl.setSrcTableName((String)map.get("SRCTBLNAME"));
		chgtbl.setTarTableName((String)map.get("TARTBLNAME"));
		chgtbl.setTarOpcond((String)map.get("TAROPCOND"));
		chgtbl.setSrcOpcond(map.get("SRCOPCOND") + "");
		return chgtbl;
	}

	@SuppressWarnings("rawtypes")
	private StoreInfo fillStoreInfo(Map map) 
	{
		StoreInfo storeInfo = new StoreInfo();
		storeInfo.setStoreNo((String)map.get("STORENO"));
		storeInfo.setMqIp((String)map.get("MQIP"));
		storeInfo.setMqUserName((String)map.get("MQUSERNAME"));
		storeInfo.setMqPwd((String)map.get("MQPWD"));
		storeInfo.setQMGName((String)map.get("QMGNAME"));
		storeInfo.setQMGPort((String)map.get("QMGPORT"));
		storeInfo.setOQName((String)map.get("OQNAME"));
		storeInfo.setIQName((String)map.get("IQNAME"));
		storeInfo.setAQName((String)map.get("AQNAME"));
		storeInfo.setQCCSID((String)map.get("QCCSID"));
		storeInfo.setDbSchema(map.get("DBSCHE") + "");
		return storeInfo;
	}
	public void addBsLog(BsLog bsLog) 
	{
		String sql = "INSERT INTO hola_app_bs_log ( ID, INSTNO, FILNAME, STATUS, REMARK, CRTTIME, STRNO) VALUES ( '" + 
								bsLog.getId() + "', '" + bsLog.getInstno() + "', '" + bsLog.getFilName() + "', '" + bsLog.getStatus() + 
								"', '" + bsLog.getRemark() + "', '" + bsLog.getCrtTime() + "', '" + bsLog.getStoreNo() + "')";
		super.getHhtserverJt().update(sql);
	}
	@SuppressWarnings("rawtypes")
	public List<String> getAllStoreNo() 
	{
		String sql = "select STORENO from hola_app_bscfg_storeinfo where  STATUS='E'";
		List list = super.getHhtserverJt().queryForList(sql);
		List<String> totle = new ArrayList<String>();
		for (Object object : list) 
		{
			Map map = (Map) object;
			totle.add(map.get("STORENO") + "");
		}
		return totle;
	}
	
}
