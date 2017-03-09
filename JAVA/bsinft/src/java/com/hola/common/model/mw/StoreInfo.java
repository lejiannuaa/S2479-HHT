package com.hola.common.model.mw;

import com.hola.common.model.BaseModel;

public class StoreInfo extends BaseModel
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private		String		storeNo;
	private		String		MqIp;
	private		String		MqUserName;
	private		String		MqPwd;
	private		String		QMGName;
	private		String		QMGPort;
	private		String		OQName;
	private		String		IQName;
	private		String		AQName;
	private		String		QCCSID;
	private		String		dbSchema;
	
	public String getStoreNo() {
		return storeNo;
	}
	public void setStoreNo(String storeNo) {
		this.storeNo = storeNo;
	}
	public String getMqIp() {
		return MqIp;
	}
	public void setMqIp(String mqIp) {
		MqIp = mqIp;
	}
	public String getMqUserName() {
		return MqUserName;
	}
	public void setMqUserName(String mqUserName) {
		MqUserName = mqUserName;
	}
	public String getMqPwd() {
		return MqPwd;
	}
	public void setMqPwd(String mqPwd) {
		MqPwd = mqPwd;
	}
	public String getQMGName() {
		return QMGName;
	}
	public void setQMGName(String qMGName) {
		QMGName = qMGName;
	}
	public String getQMGPort() {
		return QMGPort;
	}
	public void setQMGPort(String qMGPort) {
		QMGPort = qMGPort;
	}
	public String getOQName() {
		return OQName;
	}
	public void setOQName(String oQName) {
		OQName = oQName;
	}
	public String getIQName() {
		return IQName;
	}
	public void setIQName(String iQName) {
		IQName = iQName;
	}
	public String getAQName() {
		return AQName;
	}
	public void setAQName(String aQName) {
		AQName = aQName;
	}
	public String getQCCSID() {
		return QCCSID;
	}
	public void setQCCSID(String qCCSID) {
		QCCSID = qCCSID;
	}
	public String getDbSchema() {
		return dbSchema;
	}
	public void setDbSchema(String dbSchema) {
		this.dbSchema = dbSchema;
	}

}
