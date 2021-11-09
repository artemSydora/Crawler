<template>
  <div class="container">
    <h3 class="heading">Tests</h3>
    <b-table 
    id="tests" 
    striped 
    bordered
     hover
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
         <span class="left">{{ data.item.dateTime | moment("DD.MM.YYYY, hh:mm:ss") }}</span>      
      </template>
      <template #cell(details)="data" >
        <router-link :to="data.item.id + '/details'">
            <a>see details</a>
        </router-link>
      </template>
    </b-table>
    <b-pagination
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
      userInput: "",
      fields: [
        { key: "id", label: "#" },
        { key: "startPageUrl", label: "Url" },
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
        })
        .catch((error) => {
          if (error.response) {
            this.showErrorAlert = true;
            this.errorMessage = `ERROR: HTTP status "${error.response.status} ${error.response.statusText}"`;
          }
        });
    }
  },
  created() {
    this.getTestsPage(this.currentPage);
  },
};
</script>

<style>
.left {
  text-align: left;
  float: left;
}
</style>