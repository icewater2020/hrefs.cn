<template>
    <div style="position: relative;">
        <Menu tagIndex="8"></Menu>
        <div class="rightMain">
            <Header></Header>
            <div id="list">
                <div class="row">
                    <div class="col-md-12">
                        <!-- BEGIN EXAMPLE TABLE PORTLET-->
                        <div class="portlet light">
                            <div class="portlet-body">
                                <table class="table table-striped table-bordered table-hover" id="mytable">
                                    <thead>
                                        <tr>
                                            <th width="100">序号</th>
                                            <th width="200">登陆号</th>
                                            <th width="*">名字</th>
                                            <th width="100">状态</th>
                                            <th width="150">注册时间</th>
                                            <th width="150">最后登录</th>
                                            <th width="150">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="resultTable">
                                        <tr v-for="data in datas" v-bind:key="data.id">
                                            <td align="center">{{data.id}}</td>
                                            <td align="center">{{data.userid}}</td>
                                            <td><div style="max-width:785px;overflow:hidden;white-space:nowrap">{{data.user_name}}</div></td>
                                            <td align="center">{{data.status}}</td>
                                            <td align="center">{{data.reg_date | formatDate}}</td>
                                            <td>{{data.last_login_date | formatDate}}</td>
                                            <td align="center">
                                                <a v-on:click="editlink('LinkEdit', {id: data.id})" class="btn btn-sm btn-outline filter-submit purple">
                                                    <i class="fa fa-edit"></i> 修改
                                                </a>
                                                <a v-on:click="dellink(data.id)" class="btn btn-sm btn-outline filter-submit dark" style="margin-right:0;">
                                                    <i class="fa fa-lock"></i> 删除
                                                </a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <Pager ref="pager" :total="total" :current='current' :display='display' @page_change="page_change"></Pager>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import Header from '../../components/header';
    import Menu from '../../components/menu';
    import Pager from '../../components/pager';
    import httper from '../../util/httper';
    import { formatDate } from '../../util/date';
    import router from '../../router';

    export default {
        data: function () {
            return {
                datas: [],
                total: 5,
                display: 14,
                current: 1
            };
        },
        components: {
            Header,
            Menu,
            Pager
        },
        created: function () {
            let self = this;
            self.current = parseInt(self.$route.params.page);
            self.display = parseInt(self.$route.params.size);
            self.load();
        },
        filters: {
            formatDate(time) {
                let date = new Date(time);
                return formatDate(date, "yyyy-MM-dd hh:mm:ss");
            }
        },
        methods: {
            page_change: function (currentPage) {
                let self = this;
                if(self.current !== currentPage) {
                    self.current = currentPage;
                    router.push({ name: 'AccountList', params: { size: self.display, page: self.current } });
                }

                self.load();
            },
            load: function () {
                let self = this;
                httper.post('/api/account/list/' + self.display + '/' + self.current, {
                }).then(function (response) {
                    self.datas = response.data.list;
                    self.total = response.data.total;
                });
            },
            editlink: function (name, params) {
                router.push({ name: name, params: params });
            },
            dellink: function (id) {
                var self = this;
                if (confirm("确认要删除？")) {
                    httper.get('/api/account/delete/' + id).then(function (response) {
                        if (response.data.result === "1") {
                            self.load();
                        }
                    });
                }
            },
            search: function () {
                let self = this;
                self.$refs.pager.setSearch(1);
            }
        }
    };
</script>