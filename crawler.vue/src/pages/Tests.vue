<template>
  <div class="container px-0 table-margin">
    <h3 class="display-4 text-info">Tests</h3>
    <b-table
      id="tests"
      bordered
      dark
      hover
      table-variant="info"
      head-row-variant="warning"
      :items="tests"
      :fields="fields"
      :per-page="pageSize"
    >
      <template #cell(id)="data">
        {{ data.item.id }}
      </template>
      <template #cell(startPageUrl)="data">
        <span class="left">{{ data.item.startPageUrl }}</span>
      </template>
      <template #cell(dateTime)="data">
        <span class="left">{{
          data.item.dateTime | moment("DD.MM.YYYY, hh:mm:ss")
        }}</span>
      </template>
      <template #cell(details)="data">
        <router-link :to="data.item.id + '/details'">
          <a class="text-warning text-decoration">see details</a>
        </router-link>
      </template>
    </b-table>
    <b-pagination
      class="pagination"
      aria-controls="tests"
      align="center"
      @change="getTestsPage($event)"
      v-model="currentPage"
      :total-rows="totalRows"
      :per-page="pageSize"
      first-text="First"
      prev-text="Prev"
      next-text="Next"
      last-text="Last"
    ></b-pagination>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Tests",
  data() {
    return {
      tests: [],  
      currentPage: 1,
      totalPages: 1,
      pageSize: 15,
      fields: [
        { key: "id", label: "#", },
        { key: "startPageUrl", label: "Url", },
        { key: "dateTime", label: "Date" },
        { key: "details", label: "" },
      ],
    };
  },
  props:['baseUri'],
  computed:
  {
    totalRows() {
      return this.totalPages * this.pageSize;
      }
  },
  methods: {
    getTestsPage(currentPage) {
      axios
        .get(this.baseUri + `/tests?pageNumber=${currentPage}&pageSize=${this.pageSize}`)
        .then((response) => {       
          this.tests = response.data.tests;
          this.totalPages = response.data.totalPages;
        });
    }
  },
  created() {
    this.getTestsPage(this.currentPage);
  },
  mounted(){
    this.$root.$on('loadPage', () => this.getTestsPage(this.currentPage))
  }
};
</script>

<style>
.left {
  text-align: left;
  float: left;
}

.pagination {
  position: fixed; /* Фиксированное положение */
  left: 0;
  bottom: 60px; /* Левый нижний угол */
  padding: 0px;
  width: 100%; /* Ширина слоя */
}
.pagination .page-link {
  position: relative;
  display: block;
  padding: 0.5rem 0.75rem;
  margin-left: -1px;
  line-height: 1.25;
  color: #17a2b8;
  background-color: #fff;
  border: 1px solid #dee2e6;
}
.pagination .page-item.active .page-link {
  z-index: 3;
  color: #fff;
  background-color: #17a2b8;
  border-color: #17a2b8;
  box-shadow: none;
}
.table-margin {
  margin-bottom: 130px;
}
</style>