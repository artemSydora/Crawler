<template>
  <div class="container">
    <b-table striped bordered hover :items="details" :fields="fields">
      <template #cell(index)="data">
        {{ data.index + 1 }}
      </template>
      <template #cell(url)="data">
          <span class="left">{{ data.item.url }}</span>
      </template>
      <template #cell(responseTimeMs)="data">
         <span>{{ data.item.responseTimeMs }}</span>      
      </template> 
    </b-table>
 
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Details",
  data() {
    return {
      details: null,
      testId: 1,
      fields: [
        {
          key: "index",
          label: "#"
        },
        {
          key: "url",
          label: "Url",
          sortable: true
        },
        {
          key: "responseTimeMs",
          label: "Response Time (Ms)",
          sortable: true,
          'label-sort-clear':" dsd" 
        }
      ],
    };
  },
  methods: {

  },
  created() {
    axios
      .get(`http://localhost:21758/api/tests/${this.testId}/details`)
      .then((response) => {
        return (this.details = response.data);
      });
  },
};
</script>

<style>
.left {
  text-align: left;
  float: left;
}
</style>