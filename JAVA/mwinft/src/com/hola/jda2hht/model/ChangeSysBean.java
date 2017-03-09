package com.hola.jda2hht.model;

import java.io.Serializable;

/**
 * @remark 系统表的实体
 * @author 唐植超(上海软通)
 * @date 2012-12-26
 */
public class ChangeSysBean implements Serializable {
	
	public static final String DB_ORACLE = "ORACLE";
	public static final String DB_SQLSERVER = "SQLServer";

	private static final long serialVersionUID = 1L;
	// 交换系统代码
	private String syscode;
	// 系统名称
	private String sysname;
	// 数据库URL
	private String dburl;
	// 数据库连接驱动
	private String dbdriver;
	// 表空间名称
	private String dbschema;
	// 数据库用户名
	private String username;
	// 数据库连接密码
	private String password;
	// 数据库类型
	private String dbtype;
	// 状态(E:启用 D:停用)
	private String status;
	// 类型
	private String type;
	// 备注
	private String remark;
	// 新建时间
	private String createtime;

	public String getSyscode() {
		return syscode;
	}

	public void setSyscode(String syscode) {
		this.syscode = syscode;
	}

	public String getSysname() {
		return sysname;
	}

	public void setSysname(String sysname) {
		this.sysname = sysname;
	}

	public String getDburl() {
		return dburl;
	}

	public void setDburl(String dburl) {
		this.dburl = dburl;
	}

	public String getDbdriver() {
		return dbdriver;
	}

	public void setDbdriver(String dbdriver) {
		this.dbdriver = dbdriver;
	}

	public String getDbschema() {
		return dbschema;
	}

	public void setDbschema(String dbschema) {
		this.dbschema = dbschema;
	}

	public String getUsername() {
		return username;
	}

	public void setUsername(String username) {
		this.username = username;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getDbtype() {
		return dbtype;
	}

	public void setDbtype(String dbtype) {
		this.dbtype = dbtype;
	}

	public String getStatus() {
		return status;
	}

	public void setStatus(String status) {
		this.status = status;
	}

	public String getType() {
		return type;
	}

	public void setType(String type) {
		this.type = type;
	}

	public String getRemark() {
		return remark;
	}

	public void setRemark(String remark) {
		this.remark = remark;
	}

	public String getCreatetime() {
		return createtime;
	}

	public void setCreatetime(String createtime) {
		this.createtime = createtime;
	}

}
