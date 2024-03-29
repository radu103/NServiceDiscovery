sap.ui.define([
    "nservicediscovery/ui/NServiceDiscoveryUI/controller/BaseController"
], function (BaseController) {
	"use strict";

        return BaseController.extend("nservicediscovery.ui.NServiceDiscoveryUI.controller.Main", {

            onInit: function () {

            },

            onPeersItemSelected: function() {

                var oRouter = this.getRouter();
                oRouter.navTo("peers");
            },

            onTenantsItemSelected: function() {

                var oRouter = this.getRouter();
                oRouter.navTo("tenants");
            },

            onRefreshButtonPress: function() {

                var oPeersModel = this.getModel("peersModel");
                oPeersModel.loadData("/peers");

                var oTenantsModel = this.getModel("tenantsModel");
                oTenantsModel.loadData("/tenants");

            }

	});
});