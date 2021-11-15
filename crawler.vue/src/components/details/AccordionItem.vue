<template>
  <b-card no-body class="mb-1">
    <b-card-header header-tag="header" class="p-0" role="tab">
      <div
        class="bg-info w-100 text-white p-3"
        block
        v-b-toggle="id"
        variant="info"
      >
        {{ tableTitle }}
      </div>
    </b-card-header>
    <b-collapse :id="id" visible accordion="accordion" role="tabpanel">
      <b-card-body>
        <b-card-text>
          <div v-if="details.length !== 0">
            <b-table
              bordered
              dark
              hover
              responsive="lg"
              table-variant="info"
              head-row-variant="warning"
              :items="details"
              :fields="fields"
              :sort-by="sortBy"
              label-sort-clear
              label-sort-asc
              label-sort-desc
            >
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
          <div v-else>{{ errorMsg }}</div>
        </b-card-text>
      </b-card-body>
    </b-collapse>
  </b-card>
</template>
<script>
export default {
  name: "AccordionItem",
  props: {
    id: {
      type: String,
      default() {
        return null;
      },
    },
    details: {
      type: Array,
      default() {
        return [];
      },
    },
    tableTitle: {
      type: String,
      default() {
        return null;
      },
    },
    errorMsg: {
      type: String,
      default() {
        return null;
      },
    },
  },
  data() {
    return {
      sortBy: "responseTimeMs",
      fields: [
        {
          key: "index",
          label: "#",
          sortable: false,
          "head-variant": "dark",
          tdClass: "align-middle",
          thClass: "align-middle",
        },
        {
          key: "url",
          label: "Url",
          sortable: true,
          tdClass: "align-middle",
          thClass: "align-middle",
          class: "column-width"
        },
        {
          key: "responseTimeMs",
          label: "Response Time (Ms)",
          sortable: true,
          tdClass: "align-middle",
          thClass: "align-middle",
        },
      ],
    };
  },
};
</script>
<style>
.column-width {
  width: 850px;
}
</style>
