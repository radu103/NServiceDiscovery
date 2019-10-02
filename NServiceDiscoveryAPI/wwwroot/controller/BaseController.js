sap.ui.define([
    "sap/ui/core/mvc/Controller"
], function (Controller) {
    "use strict";

        return Controller.extend("nservicediscovery.ui.NServiceDiscoveryUI.controller.BaseController", {

            onSideNavButtonPress: function () {
                var oView = this.getView();
                var mainToolPage = oView.byId("mainToolPage");
                mainToolPage.setSideExpanded(!mainToolPage.getSideExpanded());
            },

            getRouter: function () {
                return sap.ui.core.UIComponent.getRouterFor(this);
            },

            getModel: function (sName) {
                return this.getView().getModel(sName);
            },

            setModel: function (oModel, sName) {
                return this.getView().setModel(oModel, sName);
            },

            getResourceBundle: function () {
                return this.getOwnerComponent().getModel("i18n").getResourceBundle();
            }
        });
});