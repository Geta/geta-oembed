// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

/* eslint-disable */
define([
    'dojo/_base/declare',

    'dijit/_WidgetBase',
    'dijit/_TemplatedMixin',
    'dijit/_WidgetsInTemplateMixin',

    'dijit/form/CheckBox',

    'dojo/text!./templates/CheckBoxWithLabel.html',
], function (
    declare,

    _WidgetBase,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,

    CheckBox,

    template,
) {
    return declare([_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
        templateString: template,

        startup: function () {
            this.inherited(arguments);

            this.connect(this.focusNode, 'onChange', 'onChange');
        },

        _getValueAttr: function () {
            return this.focusNode.get('value');
        },

        _setValueAttr: function (value) {
            this.focusNode.set('value', value);
        },

        _getCheckedAttr: function () {
            return this.focusNode.get('checked');
        },

        _setCheckedAttr: function (checked) {
            this.focusNode.set('checked', checked);
        },
    });
});
