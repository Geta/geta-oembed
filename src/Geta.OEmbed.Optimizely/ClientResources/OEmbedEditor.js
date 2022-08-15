// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

/* eslint-disable */
define([
    "dojo/_base/declare",
    "dojo/dom-construct",
    "dojo/dom-class",
    "dojo/dom-style",
    "dojo/request/xhr",

    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",

    "geta-oembed/CheckBoxWithLabel",
    "dijit/form/NumberTextBox",

    "epi-cms/widget/_HasChildDialogMixin",
    "epi/shell/widget/_ValueRequiredMixin",

    "epi-cms/widget/UrlSelector",

    "dojo/text!./templates/OEmbedEditor.html",
    "xstyle/css!./styles/geta-oembed.css",
], function (
    declare,
    domConstruct,
    domClass,
    domStyle,
    xhr,

    _WidgetBase,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,

    CheckBox,
    NumberTextBox,

    _HasChildDialogMixin,
    _ValueRequiredMixin,

    UrlSelector,

    template,
) {
    return declare(
        [
            _WidgetBase,
            _TemplatedMixin,
            _WidgetsInTemplateMixin,
            _HasChildDialogMixin,
            _ValueRequiredMixin
        ],
        {
            templateString: template,
            value: null,

            _isInitialized: false,

            constructor: function() {
                this.inherited(arguments);
                this._isInitialized = false;
            },

            destroy: function() {
                this.urlSelector.destroy();
                this.autoPlay.destroy();
                this.muted.destroy();
                this.controls.destroy();
                this.loop.destroy();
                this._preview.innerHTML = "";
                this.inherited(arguments);
            },

            postCreate: function() {
                this.inherited(arguments);
                this._createUrlSelector();
                this._createSettingsControls();
            },

            _createUrlSelector: function() {
                this.urlSelector = new UrlSelector({
                    allowedTypes: this.allowedTypes,
                    allowedDndTypes: this.allowedDndTypes,
                    dndSourcePropertyName: this.dndSourcePropertyName,
                    metadata: this.metadata,
                    repositoryKey: this.repositoryKey,
                });
                this.urlSelector.placeAt(this._urlSelectorContainer);
                this.connect(this.urlSelector, "onChange", "_onUrlChange");
                this.urlSelector.startup();
            },

            _createSettingsControls: function() {
                this.autoPlay = new CheckBox({ label: "Autoplay" });
                this.autoPlay.placeAt(this._settings);
                this.connect(this.autoPlay, "onChange", "_onAutoplayChange");
                this.autoPlay.startup();

                this.muted = new CheckBox({ label: "Muted" });
                this.muted.placeAt(this._settings);
                this.connect(this.muted, "onChange", "_onSettingsChange");
                this.muted.startup();

                this.controls = new CheckBox({ label: "Show controls" });
                this.controls.placeAt(this._settings);
                this.connect(this.controls, "onChange", "_onSettingsChange");
                this.controls.startup();

                this.loop = new CheckBox({ label: "Loop" });
                this.loop.placeAt(this._settings);
                this.connect(this.loop, "onChange", "_onSettingsChange");
                this.loop.startup();

                const container = domConstruct.create("div", { class: "geta-oembed__flex" });
                domConstruct.place(container, this._settings);

                this.maxWidth = new NumberTextBox({ label: "Max width", placeHolder: "Max width", class: "geta-oembed__input"});
                this.maxWidth.placeAt(container);
                this.connect(this.maxWidth, "onChange", "_onSettingsChange");
                this.maxWidth.startup();

                this.maxHeight = new NumberTextBox({ label: "Max height", placeHolder: "Max height", class: "geta-oembed__input"});
                this.maxHeight.placeAt(container);
                this.connect(this.maxHeight, "onChange", "_onSettingsChange");
                this.maxHeight.startup();
            },

            _equals: function (val1, val2) {
                if (val1 === val2)
                    return true;

                if (val1 && !val2)
                    return false;

                if (!val1 && val2)
                    return false;

                return val1.requestedUrl === val2.requestedUrl &&
                    val1.autoplay === val2.autoplay &&
                    val1.enableControls === val2.enableControls &&
                    val1.loop === val2.loop &&
                    val1.muted === val2.muted &&
                    val1.maxWidth === val2.maxWidth &&
                    val1.maxHeight === val2.maxHeight;
            },

            _getNumberValueOrNull: function(widget) {
                const value = widget.get("value");

                if (isNaN(value))
                    return null;

                return value;
            },

            _onAutoplayChange: function () {
                const autoplay = this.autoPlay.get("checked");
                this.muted.set("readOnly", autoplay);

                if (autoplay) {
                    this.muted.set("checked", true);
                }

                this._onSettingsChange();
            },

            _onSettingsChange: function () {
                if (this.value) {
                    this._refreshOEmbed(this.value.requestedUrl);
                }
            },

            _onUrlChange: function(value) {
                if (value === null && this.value) {
                    this._updateValue(null);
                    return;
                }

                this._refreshOEmbed(value);
            },

            _refreshOEmbed: function(oembedUrl) {
                const autoplay = this.autoPlay.get("checked");
                const controls = this.controls.get("checked");
                const loop = this.loop.get("checked");
                const muted = this.muted.get("checked");
                const maxWidth = this._getNumberValueOrNull(this.maxWidth);
                const maxHeight = this._getNumberValueOrNull(this.maxHeight);

                const newValue = {
                    requestedUrl: oembedUrl,
                    autoplay,
                    enableControls: controls,
                    loop,
                    muted,
                    maxWidth,
                    maxHeight,
                };

                if (this._equals(this.value, newValue)) {
                    return;
                }

                let requestUrl = this.endpointRoute + "?url=" + encodeURIComponent(oembedUrl) + "&autoplay=" + autoplay + "&controls=" + controls + "&loop=" + loop + "&muted=" + muted;

                if (maxWidth !== null) {
                    requestUrl += "&maxwidth=" + maxWidth;
                }

                if (maxHeight !== null) {
                    requestUrl += "&maxheight=" + maxHeight;
                }

                this.oEmbedPromise = xhr(requestUrl, { handleAs: "json" }).then((data) => {
                    data.requestedUrl = oembedUrl;
                    data.autoplay = autoplay;
                    data.enableControls = controls;
                    data.loop = loop;
                    data.muted = muted;
                    data.maxWidth = maxWidth;
                    data.maxHeight = maxHeight;
                    this._updateValue(data);
                });
            },

            _setValueAttr: function(value) {
                if (!this._started || this._equals(value, this.value)) {
                    return;
                }

                this._updatePreview(value);

                if (!this._isInitialized) {
                    if (value && value.requestedUrl) {
                        this._updateSettings(value);
                        this._updateUrlSelectorValue(value.requestedUrl);
                        this._isInitialized = true;
                    }
                }

                this._set("value", value);
            },

            _updatePreview: function(value) {
                if (value && value.requestedUrl) {
                    domClass.remove(this._preview, "dijitHidden");
                    domClass.remove(this._settings, "dijitHidden");
                    domStyle.set(this._preview, {
                        paddingBottom: `${ (value.height / value.width) * 100 }%`,
                    });

                    this._preview.innerHTML = value.html;
                } else {
                    domClass.add(this._preview, "dijitHidden");
                    domClass.add(this._settings, "dijitHidden");

                    this._preview.innerHTML = "";
                }
            },

            _updateSettings: function(value) {
                if (value === null) {
                    this._updateSettingIfChanged(this.autoPlay, false);
                    this._updateSettingIfChanged(this.controls, true);
                    this._updateSettingIfChanged(this.loop, false);
                    this._updateSettingIfChanged(this.muted, true);
                    this._updateValueIfChanged(this.maxWidth, NaN);
                    this._updateValueIfChanged(this.maxHeight, NaN);
                } else {
                    this._updateSettingIfChanged(this.autoPlay, value.autoplay);
                    this._updateSettingIfChanged(this.controls, value.enableControls);
                    this._updateSettingIfChanged(this.loop, value.loop);
                    this._updateSettingIfChanged(this.muted, value.muted);
                    this._updateValueIfChanged(this.maxWidth, value.maxWidth);
                    this._updateValueIfChanged(this.maxHeight, value.maxHeight);
                }

                this.maxWidth.set("readOnly", !value && !value.requestedUrl);
                this.maxHeight.set("readOnly", !value && !value.requestedUrl);
            },

            _updateSettingIfChanged: function(widget, checked) {
                const curr = widget.get("checked");

                if (curr !== checked) {
                    widget.set("checked", checked);
                }
            },

            _updateValueIfChanged: function(widget, value) {
                const curr = widget.get("value");

                if (curr !== value) {
                    widget.set("value", value);
                }
            },

            _updateUrlSelectorValue: function(value) {
                if (this.urlSelector.get("value") === value) {
                    return;
                }

                this.urlSelector.set("value", value);
            },

            _updateValue: function(value) {
                if (this._equals(value, this.value)) {
                    return;
                }

                this.onFocus();
                this.set("value", value);
                this.onChange(value);
                this.onBlur();
            },
        },
    );
});
