package model;

public class IntfInfo
{

	/**
	 * 
	 */
	
	private		String		IntfName;
	private		String		Status;
	private		String		SysDBUrl;
	private		String		SysDBDriver;
	private		String		DBSche;
	private		String		UserName;
	private		String		Password;
	private		String		Table;
	
	public String getIntfName() {
		return IntfName;
	}
	public void setIntfName(String IntfName) {
		this.IntfName = IntfName;
	}
	
	public String getStatus() {
		return Status;
	}
	public void setStatus(String Status) {
		this.Status = Status;
	}
	
	public String getSysDBUrl() {
		return SysDBUrl;
	}
	public void setSysDBUrl(String SysDBUrl) {
		this.SysDBUrl = SysDBUrl;
	}
	
	public String getSysDBDriver() {
		return SysDBDriver;
	}
	public void setSysDBDriver(String SysDBDriver) {
		this.SysDBDriver = SysDBDriver;
	}

	public String getDBSche() {
		return DBSche;
	}
	public void setDBSche(String DBSche) {
		this.DBSche = DBSche;
	}

	public String getUserName() {
		return UserName;
	}
	public void setUserName(String UserName) {
		this.UserName = UserName;
	}

	public String getPassword() {
		return Password;
	}
	public void setPassword(String Password) {
		this.Password = Password;
	}

	public String geTable() {
		return Table;
	}
	public void setTable(String Table) {
		this.Table = Table;
	}
	
}
