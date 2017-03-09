package com.hola.jda2hht.core.pipe.impl;

import com.hola.jda2hht.core.io.IOExecutor;
import com.hola.jda2hht.core.pipe.IPipe;
import com.hola.jda2hht.service.IChgTblDtlService;
import com.hola.jda2hht.service.IChgTblService;
import com.hola.jda2hht.service.IStoreInfoService;

public abstract class BasePipe implements IPipe {
	protected IChgTblService chgTblService;
	protected IChgTblDtlService chgTblDtlService;
	protected IStoreInfoService storeInfoService;
	protected IOExecutor executor;

	public IOExecutor getExecutor() {
		return executor;
	}

	public void setExecutor(IOExecutor executor) {
		this.executor = executor;
	}

	public IStoreInfoService getStoreInfoService() {
		return storeInfoService;
	}

	public void setStoreInfoService(IStoreInfoService storeInfoService) {
		this.storeInfoService = storeInfoService;
	}

	public IChgTblService getChgTblService() {
		return chgTblService;
	}

	public void setChgTblService(IChgTblService chgTblService) {
		this.chgTblService = chgTblService;
	}

	public IChgTblDtlService getChgTblDtlService() {
		return chgTblDtlService;
	}

	public void setChgTblDtlService(IChgTblDtlService chgTblDtlService) {
		this.chgTblDtlService = chgTblDtlService;
	}
}
