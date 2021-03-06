package com.hola.jda2hht.service.impl;

import java.util.Date;
import java.util.List;

import org.apache.log4j.Logger;

import com.hola.jda2hht.model.ChangeSysBean;
import com.hola.jda2hht.model.JDAChangeInstBean;
import com.hola.jda2hht.service.IJDAChgInstService;

/**
 * JDA ChgInst表的业务实现
 * 
 * @author 唐植超(上海软通)
 * @date 2012-12-27
 */
public class JDAInstServiceImpl extends BaseJDAService implements
		IJDAChgInstService {
	private static Logger log = Logger.getLogger(JDAInstServiceImpl.class);

	public JDAInstServiceImpl(ChangeSysBean sysBean) throws Exception {
		super(sysBean);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<JDAChangeInstBean> getInstList(String chgCode, String stuts) {
		log.info("获取所有的系统 交换编号:" + chgCode + " 状态：" + stuts);

		String sql;
		if("CRM_POS".equals(chgCode)||"POS_HD".equals(chgCode)||"EC_C1F".equals(chgCode))
		{
			sql = "select CHGCNT,CHGCOD,CHGDAT,CHGSTS,CHGTYP,CHGUSR,FILCNT,"
					+ "INSTNO,OID,PRTCOD,SRCCNT,SRCDAT,SRCSTS,SRCTIM,SRCUSR,SYSCOD,"
					+ "TRGSVR from " + schma
					+ ".CHGINST where CHGCOD =? and CHGSTS in (?, ?) and rownum<500";
		}
		else
		{
			sql = "select CHGCNT,CHGCOD,CHGDAT,CHGSTS,CHGTYP,CHGUSR,FILCNT,"
					+ "INSTNO,OID,PRTCOD,SRCCNT,SRCDAT,SRCSTS,SRCTIM,SRCUSR,SYSCOD,"
					+ "TRGSVR from " + schma
					+ ".CHGINST where CHGCOD =? and CHGSTS=? ";

		}
		
		System.out.println(sql);
		
		if("CRM_POS".equals(chgCode)||"POS_HD".equals(chgCode)||"EC_C1F".equals(chgCode))
		{
			return (List<JDAChangeInstBean>) this.dao.getList(sql, new Object[] {
					chgCode, stuts, "0" }, JDAChangeInstBean.class);
		}
		else
		{
			return (List<JDAChangeInstBean>) this.dao.getList(sql, new Object[] {
					chgCode, stuts }, JDAChangeInstBean.class);
		}
	}

	@Override
	public void updateStatus(List<JDAChangeInstBean> instList, String stuts) {
		log.info("批量更新数据状态：" + stuts);
		String sql = "update " + schma + ".CHGINST set CHGSTS=" + stuts
				+ " where INSTNO in ";
		String instnos = "'0'";
		for (JDAChangeInstBean inst : instList) {
			instnos += ",'" + inst.getInstno() + "'";
		}
		sql += "(" + instnos + ")";
		this.dao.execute(sql);
	}

	@Override
	public void updateStatus(JDAChangeInstBean inst, String stuts) {
		
		log.info("单个更新数据状态：" + stuts);
		String sql = "update " + schma + ".CHGINST set CHGSTS=" + stuts
				+ " where INSTNO ='" + inst.getInstno() + "'";
		this.dao.execute(sql);
	}

	@Override
	public void updateStatus(JDAChangeInstBean inst, String stuts, String chgtyp) {
		log.info("单个更新数据状态：" + stuts);
		String sql = "update " + schma + ".CHGINST set CHGSTS=" + stuts  +", CHGTYP='I'"
				+ " where INSTNO ='" + inst.getInstno() + "'";
		log.info("sql更新状态i"+sql);
		this.dao.execute(sql);
		
	}

}
