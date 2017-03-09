package com.hola.common.dao;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.dao.CannotAcquireLockException;

import com.hola.common.model.Chginst;
import com.hola.common.model.ChginstF;
import com.hola.common.model.mw.ChgTbl;
import com.hola.common.util.MailUtils;
import com.hola.common.util.TimeUtil;

public class StoreDao extends BaseDao 
{
	public static final String CHGINST_STS_UPLOAD_SUCESS = "2";
	public static final String CHGINST_STS_UPLOAD_NOT	 = "1";
	public static final String CHGINST_STS_UPLOADING = "0";
	
	private static Log log = LogFactory.getLog(StoreDao.class);
	/**
	 * 获取需要抛转的接口头档列表
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2012-12-27 下午3:20:02
	 * @param schema
	 * @param chgcodeStr 
	 * @return
	 */
	@SuppressWarnings("rawtypes")
	public List<Chginst> getChginstNoUpload(String schema, String chgcodeStr)
	{
		List<Chginst> totle = new ArrayList<Chginst>();
		String sql = "select INSTNO,CHGCOD from " + schema + ".CHGINST where CHGSTS='" + 
						StoreDao.CHGINST_STS_UPLOAD_NOT + "' and CHGCOD in (" + chgcodeStr + ")";// and srccnt>'0'";
		List list = super.getStoreJt().queryForList(sql);
		for (Object object : list) 
		{
			Map map = (Map) object;
			Chginst c = fillChginstForInstnoAndChgcode(map);
			totle.add(c);
			//更新接口头档为发送中,chgsts='0'
			this.updateChginstStsChgdatSyscod(schema, CHGINST_STS_UPLOADING , c.getInstno());
		}
		return totle;
	}
	/**
	 * 更新CHGINST的chgsts状态.1:未发送0:发送中2:已发送
	 * 以及syscode和chgdat三个字段
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-8 下午2:33:30
	 */
	public void updateChginstStsChgdatSyscod(String schema , String chgsts , String instno)
	{
		String sql = "update " + schema + ".CHGINST set SYSCOD='MQ' , CHGDAT='" + TimeUtil.formatDate(new Date(), TimeUtil.DATE_FORMAT_yyyyMMdd) + "' , CHGSTS='" + chgsts + "' where instno='" + instno + "'";
//		log.info(sql.toString());
		super.getStoreJt().update(sql);
	}
	/**
	 * 更新CHGINST的chgsts状态.1:未发送0:发送中2:已发送
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-8 下午2:33:30
	 */
	public void updateChginstSts(String schema , String chgsts , String instno)
	{
		String sql = "update " + schema + ".CHGINST set CHGSTS='" + chgsts + "' where instno='" + instno + "'";
		log.info(sql.toString());
		super.getStoreJt().update(sql);
	}
	@SuppressWarnings("rawtypes")
	public int getChginstCountForInstno(String instno , String schema)
	{
		String sql = "select INSTNO from " + schema + ".CHGINST where INSTNO='" + instno + "'";
		sql = super.createSqlForCount(sql);
		List list = super.getStoreJt().queryForList(sql);
		return super.getCount(list);
	}
	@SuppressWarnings("rawtypes")
	public int getChginstfCountForInstnoAndTartablename(String instno , String tarTableName , String schema)
	{
		String sql = "select INSTNO from " + schema + ".CHGINSTF where INSTNO='" + instno + "' and TARNAM='" + tarTableName + "'";
		sql = super.createSqlForCount(sql);
		List list = super.getStoreJt().queryForList(sql);
		return super.getCount(list);
	}
	public void insertChginst(String shcema, Chginst chginst)
	{
		StringBuffer sql = new StringBuffer();
		sql.append("INSERT INTO " + shcema + ".CHGINST" + 
					" (CHGCNT , CHGCOD , CHGDAT , CHGSTS , CHGTYP , CHGUSR, FILCNT,  INSTNO , OID , SRCCNT , SRCDAT ,SRCSTS, SRCTIM ,TRGSVR , SRCUSR , SYSCOD , PRTCOD) VALUES(");
		String [] params = {chginst.getChgcntInt() + "", chginst.getChgcod() , chginst.getChgdat() , chginst.getChgsts() , 
							chginst.getChgtyp() , chginst.getChgusr() , chginst.getFilcntInt() + "" , chginst.getInstno() ,
							chginst.getOid()	, chginst.getSrccntInt() + "", chginst.getSrcdat() , chginst.getSrcsts() , 
							chginst.getSrctim() , chginst.getTrgsvr() , chginst.getSrcusr() , chginst.getSyscod() , "0"};
		boolean isFirst = true;
		for (String string : params) 
		{
			if(isFirst == false)
			{
				sql.append(",");
			}
			isFirst = false;
			sql.append("'");
			sql.append(string);
			sql.append("'");
		}
		sql.append(")");
		log.info(sql.toString());
		getStoreJt().update(sql.toString());
	}
	
	/**
	 * 运行监控
	 */
	public void insertMonitorTime(String shcema, String datasheet)
	{
		StringBuffer sql = new StringBuffer();
		sql.append("INSERT INTO " + shcema + "." + datasheet +
					" (RUNNINGTIME) VALUES(");
		
		sql.append("'");
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String date = sdf.format(new Date());
		sql.append(date);
		sql.append("'");
		sql.append(")");
		log.info(sql.toString());
		getStoreJt().update(sql.toString());
	}
	
	/**
	 * 更新接口头档
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-7 下午4:43:44
	 * @param shcema
	 * @param chginst
	 */
	public void updateChginst(String shcema, Chginst chginst)
	{
		StringBuffer sql = new StringBuffer();
		sql.append("UPDATE ").append(shcema).append(".CHGINST set CHGCNT='").append(chginst.getChgcntInt() + "', SRCCNT='")
								.append(chginst.getSrccntInt() + "' where INSTNO='").append(chginst.getInstno() + "'");
		log.info(sql.toString());
		super.getStoreJt().update(sql.toString());
	}
	
	public void insertChginstF(String shcema, ChginstF chginstf)
	{
		StringBuffer sql = new StringBuffer();
		sql.append("INSERT INTO " + shcema + ".CHGINSTF" + 
					" (INSTNO ,OTHSUM , SRCCNT , SRCLIB , SRCNAM ,SRCSUM ,TARCNT , TARLIB , TARNAM ,TAROTH , TARSUM , TMPLIB , TMPNAM)  VALUES(" );
		String [] params = {chginstf.getInstno() , chginstf.getOthsum() , chginstf.getSrcCntInt() + "" , chginstf.getSrclib() , 
							chginstf.getSrcnam() , chginstf.getTarCntInt() + "" , chginstf.getTarCntInt() + "" , chginstf.getTarlib() , 
							chginstf.getTarnam() , chginstf.getTaroth() , chginstf.getTarCntInt() + "" , "" , ""};
		boolean isFirst = true;
		for (String string : params) 
		{
			if(isFirst == false)
			{
				sql.append(",");
			}
			isFirst = false;
			sql.append("'");
			sql.append(string);
			sql.append("'");
		}
		sql.append(")");
		log.info(sql.toString());
		getStoreJt().update(sql.toString());
	}
	public void updateChginstF(String shcema, ChginstF chginstf)
	{
		StringBuffer sql = new StringBuffer();
		sql.append("UPDATE ").append(shcema).append(".CHGINSTF set SRCCNT='").append(chginstf.getSrcCntInt() + "', TARCNT='")
								.append(chginstf.getTarCntInt() + "' , TARSUM='")
								.append(chginstf.getTarCntInt() + "' , SRCSUM='").append(chginstf.getSrcCntInt() + "'")
								.append(" where INSTNO='")
								.append(chginstf.getInstno() + "' and SRCNAM='").append(chginstf.getSrcnam())
								.append("' and TARNAM='").append(chginstf.getTarnam() + "'");
//		System.out.println(sql.toString());
		super.getStoreJt().update(sql.toString());
	}
	/**
	 * 通过oid和来源系统、目标系统三个字段查找chginst对象
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-7 上午10:34:02
	 * @param oid
	 * @param srcusr
	 * @param trgsvr
	 * @param schema
	 * @return
	 */
	@SuppressWarnings("rawtypes")
	public Chginst getChginstForOidSrcusrTrgsvr(String oid , String srcusr , String trgsvr , String schema)
	{
		String sql = "select * from " + schema + ".chginst where OID='" + oid + "' and SRCUSR='" + srcusr + "' and TRGSVR='" + trgsvr + "'";
		List list = super.getStoreJt().queryForList(sql);
		for (Object object : list)
		{
			return fillChginst((Map)object);
		}
		return null;
	}
	/**
	 * @deprecated
	 * 通过读取配置，更新view为已上传
	 * 方法作废。作废原因：修改view改成读取配置的方式
	 */
	public void updateViewForConfig(String schema , String instno , String chgcode)
	{
		String setSql = this.getUpdateViewSqlForChgcode().get(chgcode);
		if(setSql == null)
		{
			log.error("chgcode对应的更新条件不存在!!!chgcode:" + chgcode);
		}
		String sql = "update " + schema + "." + setSql + " where instno='" + instno + "'";
		this.getStoreJt().update(sql);
	}
	
	public void updateViewForDb(String schema , String instno , String srcOpcond)
	{
		try
		{
			String sql = "update " + schema + "." + srcOpcond + " where instno='" + instno + "'";
			log.info("更新view:" + sql);
			this.getStoreJt().update(sql);
		} catch(CannotAcquireLockException e)
		{
			MailUtils.sendMail("schema:" + schema + ";instno:" + instno + ";出现Lock异常!!!");
			log.error(e);
			throw e;
		}
		
	}
	
	@SuppressWarnings("rawtypes")
	private Chginst fillChginst(Map map)
	{
		Chginst c = new Chginst();
		c.setInstno(map.get("INSTNO") + "");
		String srcsts = map.get("SRCSTS") + "";
		c.setSrcsts(srcsts);
		c.setTrgsvr(map.get("TRGSVR") + "");
		c.setOid(map.get("OID") + "");
		c.setChgcod(map.get("CHGCOD") + "");
		
		c.setChgcnt(map.get("CHGCNT") + "");
		System.out.println(map.get("CHGCNT") + "dddddddd::::" + c.getChgcnt());
		c.setChgcntInt(Integer.parseInt(c.getChgcnt()));
		
		c.setSrccnt(map.get("SRCCNT") + "");
		c.setSrccntInt(Integer.parseInt(c.getSrccnt()));
		
		return c;
	}
	@SuppressWarnings("rawtypes")
	private Chginst fillChginstForInstnoAndChgcode(Map map)
	{
		Chginst c = new Chginst();
		c.setInstno(map.get("INSTNO") + "");
		c.setChgcod(map.get("CHGCOD") + "");
		return c;
	}
	@SuppressWarnings("rawtypes")
	public List getData(String colStr, String srcTableName, String schema , String instno)
	{
		String sql = "select " + colStr + " from " + schema + "." + srcTableName + " where instno='" + instno + "'";
		log.info("getDate:" + sql);
		return super.getStoreJt().queryForList(sql);
	}
	public int execution(String sql , Object [] params)
	{
		return this.getStoreJt().update(sql , params);
	}
	/**
	 * 根据主键在业务表中查询有几条记录
	 * @author 王成(chengwangi@isoftstone.com)
	 * @date 2013-1-5 上午10:55:31
	 * @param data
	 * @param chgtbl
	 * @param schema
	 * @return
	 */
	public int getCountForPkAndOpcond(String[] data, ChgTbl chgtbl , String schema)
	{
		String [] pkCols = chgtbl.getPkColStr().split(",");
		StringBuffer sql = new StringBuffer();
		sql.append("select ").append(chgtbl.getPkColStr()).append(" from ")
						.append(schema).append(".").append(chgtbl.getTarTableName())
						.append(" where ").append(chgtbl.getTarOpcond());
		for (int i = 0; i < pkCols.length; i++) 
		{
			sql.append(" and ");
			int pkIndex = chgtbl.getPkColNo().get(i) - 1;
			sql.append(pkCols[i]).append("='").append(data[pkIndex].trim()).append("'");
		}
		String countSql = super.createSqlForCount(sql.toString());
		log.info("判断是否执行到业务表sql：" + countSql);
		int count = super.getCount(super.getStoreJt().queryForList(countSql));
		return count;
	}
	@SuppressWarnings("rawtypes")
	public ChginstF getChginstfForInstnoSrcnamTarnam(String instno, String srcTableName, String tarTableName , String schema) 
	{
		String sql = "select INSTNO,SRCLIB,SRCNAM,SRCCNT,TARCNT,TARLIB,TARNAM from " + schema + ".CHGINSTF where INSTNO='" + instno + 
						"' and SRCNAM='" + srcTableName + "' and TARNAM='" + tarTableName + "'";
		List list = super.getStoreJt().queryForList(sql);
		for (Object object : list) 
		{
			return fillChginstf((Map)object);
		}
		return null;
	}
	@SuppressWarnings("rawtypes")
	private	ChginstF fillChginstf(Map map)
	{
		ChginstF cf = new ChginstF();
		cf.setInstno((String)map.get("INSTNO"));
		cf.setSrclib((String)map.get("SRCLIB"));
		cf.setSrcnam((String)map.get("SRCNAM"));
		
		cf.setSrccnt(map.get("SRCCNT") + "");
		cf.setSrcCntInt(Integer.parseInt(cf.getSrccnt()));
		
		cf.setTarcnt(map.get("TARCNT") + "");
		cf.setTarCntInt(Integer.parseInt(cf.getTarcnt()));
		
		cf.setTarlib((String)map.get("TARLIB"));
		cf.setTarnam((String)map.get("TARNAM"));
		return cf;
	}
}
