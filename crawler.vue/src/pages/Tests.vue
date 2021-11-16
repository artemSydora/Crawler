<template>
  <div class="container px-0 table-margin">
    <div v-if="tests.length > 0">
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
      <div v-if="totalPages > 1">
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
    </div>
    <div v-else>
      <h3 class="display-4 text-info">There are no tests yet</h3>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Tests",
  props: {
    baseUri: {
      type: String,
      default() {
        null;
      }
    }
  },
  data() {
    return {
      tests: [],
      currentPage: 1,
      totalPages: 1,
      pageSize: 15,
      needLoadLastTest: false,
      fields: [
        {
          key: "id",
          label: "#",
          tdClass: "align-middle",
          thClass: "align-middle",
          class: "id-column-width",
        },
        {
          key: "startPageUrl",
          label: "Url",
          tdClass: "align-middle",
          thClass: "align-middle",
          class: "url-column-width",
        },
        {
          key: "dateTime",
          label: "Date",
          tdClass: "align-middle",
          thClass: "align-middle",
        },
        {
          key: "details",
          label: "",
          tdClass: "align-middle",
          thClass: "align-middle",
        },
      ],
    };
  },
  computed: {
    totalRows() {
      return this.totalPages * this.pageSize;
    },
  },
  methods: {
    getTestsPage(currentPage) {
      axios
        .get(this.baseUri + `/tests?pageNumber=${currentPage}&pageSize=${this.pageSize}`)
        .then((response) => {
          this.tests = response.data.tests;
          this.totalPages = response.data.totalPages;
        });
    },
  },
  created() {
    this.getTestsPage(this.currentPage);
    this.$root.$on("loadLastTest", () => {
      this.getTestsPage(this.currentPage);
    });
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
  margin-top: 65px;
  margin-bottom: 125px;
}
.url-column-width {
  width: 700px;
}
.id-column-width {
  width: 80px;
}
</style>
