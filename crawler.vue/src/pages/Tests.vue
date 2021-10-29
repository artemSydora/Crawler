<template>
  <div class="container">
    <h3 class="heading">Tests</h3>
    <b-table striped bordered hover :items="testsPage.tests" :fields="fields">
      <template #cell(index)="data">
        {{ data.index + 1 }}
      </template>
      <template #cell(startPageUrl)="data">
          <span class="left">{{ data.item.startPageUrl }}</span>
      </template>
      <template #cell(dateTime)="data">
         <span class="left">{{ data.item.dateTime | moment("DD.MM.YYYY, hh:mm:ss") }}</span>      
      </template>
      <template #cell(details)="data" >
        <router-link :to="'/test/' + data.item.id">
            <a>see details</a>
        </router-link>
      </template>
    </b-table>
    <!-- <b-pagination
      align="center"
      v-model="currentPage"
      :total-rows="rows"
      :per-page="perPage"
      first-text="First"
      prev-text="Prev"
      next-text="Next"
      last-text="Last"
    ></b-pagination> -->
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Tests",
  data() {
    return {
      testsPage: {
        currentPage: 1,
        totalPages: 1,
        tests: [],
      },
      pageSize: 10,
      userInput: "",
      fields: [
        {
          key: "index",
          label: "#",
        },
        {
          key: "startPageUrl",
          label: "Url",
        },
        {
          key: "dateTime",
          label: "Date"
        },
        {
          key: "details",
          label: ""
        },
      ],
    };
  },
  methods: {
    getTestsPage(pageNumber) {
      axios
        .get(
          `http://localhost:21758/api/tests?pageNumber=${pageNumber}&pageSize=${this.pageSize}`
        )
        .then((response) => {
          this.testsPage = response.data;
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
    this.getTestsPage(this.testsPage.currentPage);
  },
};
</script>

<style>
.left {
  text-align: left;
  float: left;
}
</style>